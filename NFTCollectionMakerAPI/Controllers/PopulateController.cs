using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
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
    public class PopulateController : ControllerBase
    {
        private readonly IPopulateService _populateManager;
        private readonly IMapper _mapper;
        TagManager tm = new TagManager(new EfTagRepository());
        ArtworkTagManager atm = new ArtworkTagManager(new EfArtworkTagRepository());

        public PopulateController(IPopulateService populateManager, IMapper mapper)
        {
            _populateManager = populateManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Populate(int collectionID)
        {
            var response = await _populateManager.PopulateCollection(collectionID);
            var viewModelList = new List<ArtworkViewModel>();
            if (response == null)
                return NotFound();
            foreach (var item in response)
            {
                List<int> tags = atm.GetTagsByID(item.ArtworkID);
                var tagNamesList = tm.GetTagNameByArtworkID(tags);
                ArtworkViewModel viewModel = _mapper.Map<ArtworkViewModel>(item);
                viewModel.Tags = tagNamesList;
                viewModelList.Add(viewModel);
            }
            return Ok(viewModelList);
        }

        [HttpPost("deleteAll")]
        public IActionResult DeleteAll(int collectionID)
        {
            if(_populateManager.DeleteAllArtworks(collectionID))
                return Ok("Artworks Deleted");
            else
                return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
