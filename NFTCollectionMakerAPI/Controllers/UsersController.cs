using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        UserManager um = new UserManager(new EfUserRepository());
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = um.GetList();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUsers(int id)
        {
            var user = um.GetByID(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }
    }
}
