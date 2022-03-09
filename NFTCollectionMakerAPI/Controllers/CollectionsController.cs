using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
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
        public IActionResult CollectionsList()
        {
            return Ok(cm.GetList());   
        }

        [HttpGet("{userID:int}")]
        public IActionResult CollectionsOfUser(int userID)
        {
            return Ok(cm.GetCollectionsOfUser(userID));
        }

        [HttpPost]
        public IActionResult CreateCollection(Collection collection)
        {
            collection.CreatedAt = DateTime.Now;
            cm.Add(collection);
            return StatusCode(StatusCodes.Status201Created, collection.CollectionID);
        }
    }
}
