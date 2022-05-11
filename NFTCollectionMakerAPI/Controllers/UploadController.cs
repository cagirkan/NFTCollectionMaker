using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadController : ControllerBase
    {
        CollectionManager cm = new CollectionManager(new EfCollectionRepository());
        LayerTypeManager ltm = new LayerTypeManager(new EfLayerTypeRepository());
        UserManager um = new UserManager(new EfUserRepository());
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;

        public UploadController(IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _webHostEnvironment = webHostEnvironment;
            _config = config;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(int colID, int typeID)
        {
            string layerTypeName = ltm.GetByID(typeID).LayerTypeName;
            Collection collection = cm.GetByID(colID);
            var token = await HttpContext.GetTokenAsync("access_token");
            string userName = um.GetUserName(token);
            string prefix = userName + "_" + "col" + colID.ToString() + layerTypeName;
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources",
                                              "Images",
                                              "CollectionLayers",
                                              "col" + colID.ToString(),
                                              layerTypeName);
                var publicFolderName = Path.Combine("CollectionLayers",
                                              "col" + colID.ToString(),
                                              layerTypeName);
                var pathToSave = Path.Combine(_webHostEnvironment.ContentRootPath,"wwwroot", folderName);
                Directory.CreateDirectory(pathToSave);
                if (file.Length > 0)
                {
                    var fileName = prefix + "_" + DateTime.Now.ToString("MMddhhmmss") + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var publicPath = Path.Combine("Resources","Images", publicFolderName, fileName);
                    var imagePath = Path.Combine(_config.GetValue<string>("ServerUrl"), publicPath);
                    imagePath = imagePath.Replace("\\", "/");
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return Ok(imagePath);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
