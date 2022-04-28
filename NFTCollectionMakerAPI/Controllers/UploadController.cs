﻿using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(int colID, int typeID)
        {
            string layerTypeName = ltm.GetByID(typeID).LayerTypeName;
            Collection collection = cm.GetByID(colID);
            var token = await HttpContext.GetTokenAsync("access_token");
            var key = Encoding.ASCII.GetBytes("guessing this key shouldn't be too hard by icagirkan");
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            string userName = claims.Identity.Name;
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
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                Directory.CreateDirectory(pathToSave);
                if (file.Length > 0)
                {
                    var fileName = prefix + "_" + DateTime.Now.ToString("MMddhhmmss") + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var publicPath = Path.Combine("img", publicFolderName, fileName);
                    var imagePath = Path.Combine("https://localhost:44386", publicPath); //link will change with base url
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
