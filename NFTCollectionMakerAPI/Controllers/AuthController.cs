using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        UserManager um = new UserManager(new EfUserRepository());

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody]User user)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var token = authService.Authenticate(user.UserName, user.Password);
            var userID = um.getIdByUsername(user.UserName);
            if(token == null || user.UserName == null || user.Password == null)
            {
                return Unauthorized();
            }
            result.Add("token", token.ToString());
            result.Add("user", user.UserName);
            result.Add("userID", userID.ToString());
            return Ok(result);
        }

        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            UserValidator validationRules = new UserValidator();
            var result = validationRules.Validate(user);
            if (result.IsValid)
            {
                user.CreatedAt = DateTime.Now;
                um.Add(user);
                return StatusCode(StatusCodes.Status201Created, user);

            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                //To return an error message in the same template as the API
                var errors = new Dictionary<string, object>();
                var errorList = ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                errors.Add("errors", errorList);
                return StatusCode(StatusCodes.Status400BadRequest, errors);
            }
        }
    }
}
