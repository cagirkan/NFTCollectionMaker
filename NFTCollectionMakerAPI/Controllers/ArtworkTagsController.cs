using BusinessLayer.Concrete;
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
    public class ArtworkTagsController : ControllerBase
    {
        ArtworkTagManager atm = new ArtworkTagManager(new EfArtworkTagRepository());
        [HttpGet]
        public IActionResult GetArtworkTags()
        {
            var artworkTags = atm.GetList();
            return Ok(artworkTags);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetArtworkTags(int id)
        {
            var artworkTag = atm.GetByID(id);
            if (artworkTag == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(artworkTag);
            }
        }

        [HttpPost]
        public IActionResult CreateArtworkTag(ArtworkTag artworkTag)
        {
            artworkTag.CreatedAt = DateTime.Now;
            atm.Add(artworkTag);
            return StatusCode(StatusCodes.Status201Created, artworkTag.ArtworkTagID);
        }

        [HttpPut]
        public IActionResult EditArtworkTag(ArtworkTag artworkTag)
        {
            atm.Update(artworkTag);
            return StatusCode(StatusCodes.Status201Created, artworkTag);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteArtworkTag(int id)
        {
            var artworkTag = atm.GetByID(id);
            if (artworkTag == null)
            {
                return NotFound();
            }
            else
            {
                atm.Delete(artworkTag);
                return Ok();
            }
        }
    }
}

