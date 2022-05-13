using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectionLayersController : ControllerBase
    {
        CollectionLayerManager clm = new CollectionLayerManager(new EfCollectionLayerRepository());
        UserManager um = new UserManager(new EfUserRepository());
        CollectionManager cm = new CollectionManager(new EfCollectionRepository());
        TagManager tm = new TagManager(new EfTagRepository());
        LayerTagManager ltm = new LayerTagManager(new EfLayerTagRepository());
        LayerTypeManager ltym = new LayerTypeManager(new EfLayerTypeRepository());
        private readonly IPopulateService _populateManager;
        private readonly IConfiguration _config;

        public CollectionLayersController(IPopulateService populateManager, IConfiguration configuration)
        {
            _populateManager = populateManager;
            _config = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetCollectionLayers()
        {
            var userID = um.GetUser(await HttpContext.GetTokenAsync("access_token")).UserID;
            var collectionLayers = clm.GetCollectionLayersOfUser(userID);
            return Ok(collectionLayers);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCollectionLayers(int id)
        {
            var userID = um.GetUser(await HttpContext.GetTokenAsync("access_token")).UserID;
            CollectionLayer collectionLayer = clm.GetByIDAuth(id, userID);
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
            collectionLayer.CreatedAt = DateTime.Now;
            collectionLayer.UpdatedAt = DateTime.Now;
            collectionLayer.Popularity = 0;
            if (collectionLayer.LayerIndex == 0)
                collectionLayer.LayerIndex = 100;
            LayerType layerType = ltym.GetByID(collectionLayer.LayerTypeID);
            Collection collection = cm.GetByID(collectionLayer.CollectionID);
            string filename = collectionLayer.ImageURL.Substring(collectionLayer.ImageURL.LastIndexOf("/") + 1);
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                                       "wwwroot",
                                       "Resources",
                                       "Images",
                                       "CollectionLayers",
                                       "col" + collection.CollectionID.ToString(),
                                       layerType.LayerTypeName,
                                       filename);
            collectionLayer.ImagePath = path;
            collectionLayer.CollectionLayerName = filename.Substring(0, filename.Length - 4);
            int collectionLayerID = clm.AddWithReturn(collectionLayer);
            //Create Tag
            string tag = await _populateManager.GetTag(collectionLayer.ImageURL);
            Tag dbTag = new Tag();
            dbTag.TagName = tag;
            dbTag.CreatedAt = DateTime.Now;
            dbTag.UpdatedAt = DateTime.Now;
            int tagID = tm.AddWithReturn(dbTag);
            //Create LayerTag
            LayerTag layerTag = new LayerTag();
            layerTag.CreatedAt = DateTime.Now;
            layerTag.UpdatedAt = DateTime.Now;
            layerTag.CollectionLayerID = collectionLayerID;
            layerTag.TagID = tagID;
            ltm.Add(layerTag);
            return StatusCode(StatusCodes.Status201Created, tag);
        }

        [HttpPut]
        public IActionResult EditCollectionLayer(CollectionLayer collectionLayer)
        {
            CollectionLayerValidator validationRules = new CollectionLayerValidator();
            ValidationResult result = validationRules.Validate(collectionLayer);

            if (result.IsValid)
            {
                collectionLayer.UpdatedAt = DateTime.Now;
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
                if ((System.IO.File.Exists(collectionLayer.ImagePath)))
                {
                    System.IO.File.Delete(collectionLayer.ImagePath);
                }
                clm.Delete(collectionLayer);
                return Ok();
            }
        }
    }
}
