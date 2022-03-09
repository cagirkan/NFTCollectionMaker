using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
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
            var values = cm.GetList();
            return Ok(values);
        }

        [HttpGet("{userID:int}")]
        public IActionResult CollectionsOfUser(int userID)
        {
            var values = cm.GetCollectionsOfUser(userID);
            return Ok(values);
        }
    }
}
