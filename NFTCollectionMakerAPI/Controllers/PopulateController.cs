using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NFTCollectionMakerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PopulateController : ControllerBase
    {
        private readonly IPopulateService _populateManager;

        public PopulateController(IPopulateService populateManager)
        {
            _populateManager = populateManager;
        }

        [HttpPost]
        public async Task<IActionResult> Populate(int collectionID)
        {
            var response = await _populateManager.PopulateCollection(collectionID);
            if (response == null)
                return NotFound();
            return Ok(response);
        }
    }
}
