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
    public class LayerTagsController : ControllerBase
    {
        LayerTagManager ltm = new LayerTagManager(new EfLayerTagRepository());
        [HttpGet]
        public IActionResult GetLayerTags()
        {
            var layerTags = ltm.GetList();
            return Ok(layerTags);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetLayerTags(int id)
        {
            var layerTag = ltm.GetByID(id);
            if (layerTag == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(layerTag);
            }
        }

        [HttpPost]
        public IActionResult CreateLayerTag(LayerTag layerTag)
        {
            layerTag.CreatedAt = DateTime.Now;
            ltm.Add(layerTag);
            return StatusCode(StatusCodes.Status201Created, layerTag.LayerTagID);
        }

        [HttpPut]
        public IActionResult EditLayerTag(LayerTag layerTag)
        {
            ltm.Update(layerTag);
            return StatusCode(StatusCodes.Status202Accepted, layerTag);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteLayerTag(int id)
        {
            var layerTag = ltm.GetByID(id);
            if (layerTag == null)
            {
                return NotFound();
            }
            else
            {
                ltm.Delete(layerTag);
                return Ok();
            }
        }
    }
}
