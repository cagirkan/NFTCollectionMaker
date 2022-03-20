using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworkLayersController : ControllerBase
    {
        ArtworkLayerManager alm = new ArtworkLayerManager(new EfArtworkLayerRepository());
        [HttpGet]
        public IActionResult GetTags()
        {
            var artworkLayers = alm.GetList();
            return Ok(artworkLayers);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetTags(int id)
        {
            var artworkLayer = alm.GetByID(id);
            if (artworkLayer == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(artworkLayer);
            }
        }

        [HttpPost]
        public IActionResult CreateTag(ArtworkLayer artworkLayer)
        {
            artworkLayer.CreatedAt = DateTime.Now;
            alm.Add(artworkLayer);
            return StatusCode(StatusCodes.Status201Created, artworkLayer.ArtworkLayerID);
        }

        [HttpPut]
        public IActionResult EditTag(ArtworkLayer artworkLayer)
        {
            alm.Update(artworkLayer);
            return StatusCode(StatusCodes.Status202Accepted, artworkLayer);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTag(int id)
        {
            var artworkLayer = alm.GetByID(id);
            if (artworkLayer == null)
            {
                return NotFound();
            }
            else
            {
                alm.Delete(artworkLayer);
                return Ok();
            }
        }
    }
}
