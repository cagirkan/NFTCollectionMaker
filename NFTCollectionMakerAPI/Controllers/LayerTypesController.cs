using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LayerTypesController : ControllerBase
    {
        LayerTypeManager ltm = new LayerTypeManager(new EfLayerTypeRepository());
        UserManager um = new UserManager(new EfUserRepository());
        [HttpGet]
        public async Task<IActionResult> GetLayerTypes()
        {
            var userID = um.GetUser(await HttpContext.GetTokenAsync("access_token")).UserID;
            var layerTypes = ltm.GetLayerTypesOfUser(userID);
            if(layerTypes == null)
                return NotFound();
            else
                return Ok(layerTypes);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetLayerTypes(int id)
        {
            var userID = um.GetUser(await HttpContext.GetTokenAsync("access_token")).UserID;
            var layerType = ltm.GetByIDAuth(id, userID);
            if (layerType == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(layerType);
            }
        }

        [HttpPost]
        public IActionResult CreateLayerType(LayerType layerType)
        {
            layerType.CreatedAt = DateTime.Now;
            if (layerType.LayerTypeName == null)
                layerType.LayerTypeName = "New Layer";
            ltm.Add(layerType);
            return StatusCode(StatusCodes.Status201Created, layerType);
        }

        [HttpPut]
        public IActionResult EditLayerType(LayerType layerType)
        {
            layerType.UpdatedAt = DateTime.Now;
            ltm.Update(layerType);
            return StatusCode(StatusCodes.Status202Accepted, layerType);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteLayerType(int id)
        {
            var layerType = ltm.GetByID(id);
            if (layerType == null)
            {
                return NotFound();
            }
            else
            {
                ltm.Delete(layerType);
                return Ok();
            }
        }
    }
}
