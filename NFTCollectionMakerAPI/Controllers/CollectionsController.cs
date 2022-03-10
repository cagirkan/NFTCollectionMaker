using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
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
    public class CollectionsController : ControllerBase
    {
        CollectionManager cm = new CollectionManager(new EfCollectionRepository());

        [HttpGet]
        public IActionResult GetCollections()
        {
            return Ok(cm.GetList());   
        }

        [HttpGet("{userID:int}")]
        public IActionResult GetCollectionsOfUser(int userID)
        {
            return Ok(cm.GetCollectionsOfUser(userID));
        }

        //User id kontrolü yapılacak mı?
        [HttpPost]
        public IActionResult CreateCollection(Collection collection)
        {
            CollectionValidator validationRules = new CollectionValidator();
            ValidationResult result = validationRules.Validate(collection);

            if (result.IsValid)
            {
                collection.CreatedAt = DateTime.Now;
                cm.Add(collection);
                return StatusCode(StatusCodes.Status201Created, collection.CollectionID);
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
        public IActionResult DeleteCollection(int id)
        {
            var collection = cm.GetByID(id);
            if (collection == null)
            {
                return NotFound();
            }
            else
            {
                cm.Delete(collection);
                return Ok();
            }
        }

    }
}
