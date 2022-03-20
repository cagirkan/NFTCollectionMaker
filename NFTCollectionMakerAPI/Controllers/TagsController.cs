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
    public class TagsController : ControllerBase
    {
        TagManager tm = new TagManager(new EfTagRepository());
        [HttpGet]
        public IActionResult GetTags()
        {
            var tags = tm.GetList();
            return Ok(tags);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetTags(int id)
        {
            var tag = tm.GetByID(id);
            if (tag == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(tag);
            }
        }

        [HttpPost]
        public IActionResult CreateTag(Tag tag)
        {
            TagValidator validationRules = new TagValidator();
            ValidationResult result = validationRules.Validate(tag);

            if (result.IsValid)
            {
                tag.CreatedAt = DateTime.Now;
                tm.Add(tag);
                return StatusCode(StatusCodes.Status201Created, tag.TagID);
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
        public IActionResult EditTag(Tag tag)
        {
            TagValidator validationRules = new TagValidator();
            ValidationResult result = validationRules.Validate(tag);

            if (result.IsValid)
            {
                tm.Update(tag);
                return StatusCode(StatusCodes.Status202Accepted, tag);
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
        public IActionResult DeleteTag(int id)
        {
            var tag = tm.GetByID(id);
            if (tag == null)
            {
                return NotFound();
            }
            else
            {
                tm.Delete(tag);
                return Ok();
            }
        }
    }
}
