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
    public class LayerTypesController : ControllerBase
    {
        LayerTypeManager ltm = new LayerTypeManager(new EfLayerTypeRepository());
        [HttpGet]
        public IActionResult GetTags()
        {
            var layerTypes = ltm.GetList();
            return Ok(layerTypes);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetTags(int id)
        {
            var layerType = ltm.GetByID(id);
            if (layerType == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(layerType);
            }
        }

        [HttpPost]
        public IActionResult CreateTag(LayerType layerType)
        {
            layerType.CreatedAt = DateTime.Now;
            ltm.Add(layerType);
            return StatusCode(StatusCodes.Status201Created, layerType.LayerTypeID);
        }

        [HttpPut]
        public IActionResult EditTag(LayerType layerType)
        {
            ltm.Update(layerType);
            return StatusCode(StatusCodes.Status202Accepted, layerType);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTag(int id)
        {
            var layerType = ltm.GetByID(id);
            if (layerType == null)
            {
                return NotFound();
            }
            else
            {
                ltm.Delete(layerType);
                return Ok();
            }
        }
    }
}
