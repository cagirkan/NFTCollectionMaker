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
        public IActionResult GetLayerTypes()
        {
            var layerTypes = ltm.GetList();
            return Ok(layerTypes);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetLayerTypes(int id)
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
        public IActionResult CreateLayerType(LayerType layerType)
        {
            layerType.CreatedAt = DateTime.Now;
            if (layerType == null)
                layerType.LayerTypeName = "New Layer";
            ltm.Add(layerType);
            return StatusCode(StatusCodes.Status201Created, layerType.LayerTypeID);
        }

        [HttpPut]
        public IActionResult EditLayerType(LayerType layerType)
        {
            layerType.UpdatedAt = DateTime.Now;
            ltm.Update(layerType);
            return StatusCode(StatusCodes.Status202Accepted, layerType);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteLayerType(int id)
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
