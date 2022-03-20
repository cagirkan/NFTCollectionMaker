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
        public IActionResult GetTags()
        {
            var layerTags = ltm.GetList();
            return Ok(layerTags);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetTags(int id)
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
        public IActionResult CreateTag(LayerTag layerTag)
        {
            layerTag.CreatedAt = DateTime.Now;
            ltm.Add(layerTag);
            return StatusCode(StatusCodes.Status201Created, layerTag.TagID);
        }

        [HttpPut]
        public IActionResult EditTag(LayerTag layerTag)
        {
            ltm.Update(layerTag);
            return StatusCode(StatusCodes.Status201Created, layerTag);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTag(int id)
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
