using AutoMapper;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFTCollectionMakerAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArtworksController : ControllerBase
    {
        private readonly IMapper _mapper;
        ArtworkManager am = new ArtworkManager(new EfArtworkRepository());
        UserManager um = new UserManager(new EfUserRepository());
        ArtworkTagManager atm = new ArtworkTagManager(new EfArtworkTagRepository());
        TagManager tm = new TagManager(new EfTagRepository());
        Context c = new Context();

        public ArtworksController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtworks()
        {
            var userID = um.GetUser(await HttpContext.GetTokenAsync("access_token")).UserID;
            List<Artwork> artworks = am.GetArtworkssOfUser(userID);
            return Ok(artworks);
        }

        [HttpGet("{ID:int}")]
        public async Task<IActionResult> GetArtworks(int id)
        {
            var userID = um.GetUser(await HttpContext.GetTokenAsync("access_token")).UserID;
            var artwork = c.Artworks.Include(x => x.ArtworkLayers).Include(x => x.Collection).Where(x => x.ArtworkID == id).FirstOrDefault();
            if (artwork == null)
            {
                return NotFound();
            }
            else if (artwork.Collection.UserId != userID)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(artwork);
            }
        }

        [HttpGet("CollectionID/{collectionID:int}")]
        public IActionResult GetArtworksOfCollection(int collectionID)
        {
            var viewModelList = new List<ArtworkViewModel>();
            var artworks = am.GetByCollectionID(collectionID);
            foreach (var item in artworks)
            {
                List<int> tags = atm.GetTagsByID(item.ArtworkID);
                var tagNamesList = tm.GetTagNameByArtworkID(tags).Distinct().ToList();
                ArtworkViewModel viewModel = _mapper.Map<ArtworkViewModel>(item);
                viewModel.Tags = tagNamesList;
                viewModelList.Add(viewModel);
            }
            return Ok(viewModelList);
        }

        [HttpPost]
        public IActionResult CreateArtwork(Artwork artwork)
        {
            ArtworkValidator validationRules = new ArtworkValidator();
            ValidationResult result = validationRules.Validate(artwork);
            if (result.IsValid)
            {
                artwork.CreatedAt = DateTime.Now;
                am.Add(artwork);
                return StatusCode(StatusCodes.Status201Created, artwork);
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
        }

        [HttpPut]
        public IActionResult EditArtwork(Artwork artwork)
        {
            ArtworkValidator validationRules = new ArtworkValidator();
            ValidationResult result = validationRules.Validate(artwork);
            
                am.Update(artwork);
                return Ok(artwork);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCollection(int id)
        {
            var artwork = am.GetByID(id);
            if (artwork == null)
                return NotFound();
            if ((System.IO.File.Exists(artwork.ImagePath)))
            {
                System.IO.File.Delete(artwork.ImagePath);
            }
            am.Delete(artwork);
            return Ok();
        }
    }
}
