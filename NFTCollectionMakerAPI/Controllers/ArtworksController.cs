using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
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
            artwork.CreatedAt = DateTime.Now;
            am.Add(artwork);
            return StatusCode(StatusCodes.Status201Created, artwork);
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
