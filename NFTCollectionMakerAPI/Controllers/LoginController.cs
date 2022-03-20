using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService authService;

        public LoginController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody]User user)
        {
            var token = authService.Authenticate(user.UserName, user.Password);
            if(token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }

    }
}
