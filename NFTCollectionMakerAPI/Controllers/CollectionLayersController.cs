﻿using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionLayersController : ControllerBase
    {
        CollectionLayerManager clm = new CollectionLayerManager(new EfCollectionLayerRepository());
        TagManager tm = new TagManager(new EfTagRepository());
        LayerTagManager ltm = new LayerTagManager(new EfLayerTagRepository());
        private readonly IPopulateService _populateManager;

        public CollectionLayersController(IPopulateService populateManager)
        {
            _populateManager = populateManager;
        }

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
        public async Task<IActionResult> CreateCollectionLayer(CollectionLayer collectionLayer)
        {
            CollectionLayerValidator validationRules = new CollectionLayerValidator();
            ValidationResult result = validationRules.Validate(collectionLayer);

            if (result.IsValid)
            {
                collectionLayer.CreatedAt = DateTime.Now;
                collectionLayer.UpdatedAt = DateTime.Now;
                collectionLayer.Popularity = 0;
                collectionLayer.LayerIndex = 100;
                int collectionLayerID = clm.AddWithReturn(collectionLayer);
                //Create Tag
                string tag = await _populateManager.GetTag(collectionLayer.ImageURL);
                Tag dbTag = new Tag();
                dbTag.TagName = tag;
                int tagID = tm.AddWithReturn(dbTag);
                //Create LayerTag
                LayerTag layerTag = new LayerTag();
                layerTag.CreatedAt = DateTime.Now;
                layerTag.UpdatedAt = DateTime.Now;
                layerTag.CollectionLayerID = collectionLayerID;
                layerTag.TagID = tagID;
                ltm.Add(layerTag);
                return StatusCode(StatusCodes.Status201Created, collectionLayer);
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
