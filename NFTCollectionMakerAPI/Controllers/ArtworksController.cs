using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworksController : ControllerBase
    {
        ArtworkManager am = new ArtworkManager(new EfArtworkRepository());
        [HttpGet]
        public IActionResult GetArtworks()
        {
            return Ok(am.GetList());
        }

        [HttpGet("{ID:int}")]
        public IActionResult GetArtworks(int id)
        {
            return Ok(am.GetByID(id));
        }

        [HttpGet("CollectionID/{collectionID:int}")]
        public IActionResult GetArtworksOfCollection(int collectionID)
        {
            return Ok(am.GetByCollectionID(collectionID));
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

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCollection(int id)
        {
            var artwork = am.GetByID(id);
            am.Delete(artwork);
            return Ok();
        }
    }
}
