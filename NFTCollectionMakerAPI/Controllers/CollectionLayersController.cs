using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionLayersController : ControllerBase
    {
        CollectionLayerManager clm = new CollectionLayerManager(new EfCollectionLayerRepository());
        [HttpGet]
        public IActionResult GetCollectionLayers()
        {
            var collectionLayers = clm.GetList();
            return Ok(collectionLayers);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCollectionLayers(int id)
        {
            var collectionLayer = clm.GetByID(id);
            if (collectionLayer == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(collectionLayer);
            }
        }

        [HttpPost]
        public IActionResult CreateCollectionLayer(CollectionLayer collectionLayer)
        {
            CollectionLayerValidator validationRules = new CollectionLayerValidator();
            ValidationResult result = validationRules.Validate(collectionLayer);

            if (result.IsValid)
            {
                collectionLayer.CreatedAt = DateTime.Now;
                clm.Add(collectionLayer);
                return StatusCode(StatusCodes.Status201Created, collectionLayer.CollectionLayerID);
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
        public IActionResult EditCollectionLayer(CollectionLayer collectionLayer)
        {
            CollectionLayerValidator validationRules = new CollectionLayerValidator();
            ValidationResult result = validationRules.Validate(collectionLayer);

            if (result.IsValid)
            {
                clm.Update(collectionLayer);
                return StatusCode(StatusCodes.Status202Accepted, collectionLayer);
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
        public IActionResult DeleteCollectionLayer(int id)
        {
            var collectionLayer = clm.GetByID(id);
            if (collectionLayer == null)
            {
                return NotFound();
            }
            else
            {
                clm.Delete(collectionLayer);
                return Ok();
            }
        }
    }
}
