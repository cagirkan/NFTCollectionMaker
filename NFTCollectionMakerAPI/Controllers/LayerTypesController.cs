using BusinessLayer.Concrete;
using BusinessLayer.Constants;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
        CollectionLayerManager clm = new CollectionLayerManager(new EfCollectionLayerRepository());
        CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());
        Context c = new Context();
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
            var layerType = c.LayerTypes
                .Where(x => x.LayerTypeID == id)
                .Include(x => x.CollectionLayers).ThenInclude(x => x.LayerTags).ThenInclude(x => x.Tag);
                ;
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
                clm.DeleteLayersOfType(id);
                CollectionAnalytic collectionAnalytic = cam.GetByKey(layerType.CollectionID, Constants.Analytics.ArtworksWith + char.ToUpper(layerType.LayerTypeName[0]) + layerType.LayerTypeName.Substring(1));
                if(collectionAnalytic != null)
                    cam.Delete(collectionAnalytic);
                ltm.Delete(layerType);
                return Ok();
            }
        }
    }
}
