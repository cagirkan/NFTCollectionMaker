using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulateController : ControllerBase
    {
        private readonly IPopulateService _populateManager;

        public PopulateController(IPopulateService populateManager)
        {
            _populateManager = populateManager;
        }

        [HttpPost]
        public IActionResult Populate(int collectionID)
        {
            _populateManager.PopulateCollection(collectionID);
            return null;
        }
    }
}
