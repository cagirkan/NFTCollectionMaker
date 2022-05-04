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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFTCollectionMakerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionsController : ControllerBase
    {
        CollectionManager cm = new CollectionManager(new EfCollectionRepository());
        CollectionLayerManager clm = new CollectionLayerManager(new EfCollectionLayerRepository());
        ArtworkManager am = new ArtworkManager(new EfArtworkRepository());
        UserManager um = new UserManager(new EfUserRepository());
        Context c = new Context();
        [HttpGet]
        public IActionResult GetCollections([FromHeader] object obj)
        {
            var collectionsList = c.Collections.Include(x => x.Artworks);
            foreach (Collection item in collectionsList)
            {
                foreach (var artwork in item.Artworks)
                {
                    if (artwork.ImageURL != null)
                    {
                        item.CoverImage = artwork.ImageURL;
                        break;
                    }
                }
            }
            return Ok(collectionsList);
        }

        [HttpGet("{collectionID:int}")]
        public IActionResult GetCollectionWithArtwork(int collectionID)
        {
            var collection = c.Collections.Include(x => x.Artworks).Where(x => x.CollectionID == collectionID).FirstOrDefault();
            if (collection == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(collection);
            }
        }


        [HttpPost]
        public IActionResult CreateCollection(Collection collection)
        {
            collection.CollectionName = "New Collection";
            CollectionValidator validationRules = new CollectionValidator();
            ValidationResult result = validationRules.Validate(collection);
            if (result.IsValid)
            {
                collection.CreatedAt = DateTime.Now;
                cm.Add(collection);
                return StatusCode(StatusCodes.Status201Created, collection);
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
        public async Task<IActionResult> EditCollectionAsync(Collection collection)
        {
            CollectionValidator validationRules = new CollectionValidator();
            ValidationResult result = validationRules.Validate(collection);
            var token = await HttpContext.GetTokenAsync("access_token");
            string userName = um.GetUserName(token);
            if (result.IsValid)
            {
                collection.UserId = um.getIdByUsername(userName);
                collection.UpdatedAt = DateTime.Now;
                cm.Update(collection);
                return Ok(collection);
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

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCollection(int id)
        {
            var collection = cm.GetByID(id);
            if (collection == null)
                return NotFound();
            var collecctionLayers = clm.GetLayersOfCollection(id);
            var artworks = am.GetByCollectionID(id);
            
            foreach (var item in collecctionLayers)
                if ((System.IO.File.Exists(item.ImagePath)))
                    System.IO.File.Delete(item.ImagePath);

            foreach (var item in artworks)
                if ((System.IO.File.Exists(item.ImagePath)))
                    System.IO.File.Delete(item.ImagePath);


            cm.Delete(collection);
            return Ok();

        }

    }
}
