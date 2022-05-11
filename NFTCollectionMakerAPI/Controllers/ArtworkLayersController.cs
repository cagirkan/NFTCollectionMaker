using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArtworkLayersController : ControllerBase
    {
        ArtworkLayerManager alm = new ArtworkLayerManager(new EfArtworkLayerRepository());
        [HttpGet]
        public IActionResult GetArtworkLayers()
        {
            var artworkLayers = alm.GetList();
            return Ok(artworkLayers);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetArtworkLayers(int id)
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
        public IActionResult CreateArtworkLayer(ArtworkLayer artworkLayer)
        {
            artworkLayer.CreatedAt = DateTime.Now;
            alm.Add(artworkLayer);
            return StatusCode(StatusCodes.Status201Created, artworkLayer.ArtworkLayerID);
        }

        [HttpPut]
        public IActionResult EditArtworkLayer(ArtworkLayer artworkLayer)
        {
            alm.Update(artworkLayer);
            return StatusCode(StatusCodes.Status202Accepted, artworkLayer);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteArtworkLayer(int id)
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
