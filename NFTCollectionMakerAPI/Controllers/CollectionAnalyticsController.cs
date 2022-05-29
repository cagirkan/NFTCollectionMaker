using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFTCollectionMakerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollectionAnalyticsController : ControllerBase
    {
        CollectionAnalyticManager cam = new CollectionAnalyticManager(new EfCollectionAnalyticRepository());
        ArtworkTagManager atm = new ArtworkTagManager(new EfArtworkTagRepository());
        TagManager tm = new TagManager(new EfTagRepository());
        ArtworkManager am = new ArtworkManager(new EfArtworkRepository());

        [HttpGet("{id:int}")]
        public IActionResult GetCollectionAnalytics(int id)
        {
            Dictionary<string, object> responseData = new Dictionary<string, object>();
            Dictionary<string,int> collectionTags = new Dictionary<string, int>();
            List<CollectionAnalytic> collectionAnalytics = cam.GetByCollectionID(id);
            if (collectionAnalytics == null)
            {
                return NotFound();
            }
            responseData.Add("collectionAnalytics", collectionAnalytics);

            var artworks = am.GetByCollectionID(id);

            foreach (var item in artworks)
            {
                 var artworkTags = atm.GetTagsOfArtworks(item.ArtworkID);
                foreach (var artworkTag in artworkTags)
                {
                    var tag = tm.GetByID(artworkTag.TagID);
                    if(collectionTags.ContainsKey(tag.TagName))
                        collectionTags[tag.TagName] += 1;
                    else
                        collectionTags.Add(tag.TagName, 1);
                }
            }

            responseData.Add("collectionTags", collectionTags);

            return Ok(responseData);
        }

        [HttpGet("ArtworkTags/{collectionID:int}")]
        public IActionResult GetArtworkTagAnalytics(int id)
        {
            
            return Ok();
        }
    }
}
