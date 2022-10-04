Efficient efficie
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Works_API.Repositories;
using Works_API.Models.DTO;
using Works_API.Models.Domain;

namespace Works_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {

            var walks = await walkRepository.GetAllAsync();


            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalknAsync(Guid id)
        {
            // Get walk Domain object from database
            var walkDomain = await walkRepository.GetAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            // Request(DTO) to Domain model

            var walk = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,

            };

            // Pass details to Repositpory
            walk = await walkRepository.AddAsync(walk);

            // Convert back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId,
            };

            return CreatedAtAction(nameof(GetWalknAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //Get a region from database
            var walk = await walkRepository.DeleteAsync(id);


            //if null NotFound
            if (walk == null)
            {
                return NotFound();
            }
            //Convert response back to DTO
            var walkDTO = new Models.DTO.Walk()
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId,
            };
            //return Ok response
            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest UpdateWalkRequest)
        {
            //Convert DTO to Domain Model
            var walk = new Models.Domain.Walk()
            {
                Name = UpdateWalkRequest.Name,
                Length = UpdateWalkRequest.Length,
                
            };

            //Update Region Using Repository
            var walkUpdate = await walkRepository.UpdateAsync(id, walk);

            //If  null thyen Not Found
            if (walkUpdate == null)
            {
                return NotFound();
            }
            var walkDTO = new Models.DTO.Walk()
            {
                Id = walkUpdate.Id,
                Name=walkUpdate.Name,
                Length = walkUpdate.Length,
                RegionId = walkUpdate.RegionId,
                WalkDifficultyId =walkUpdate.WalkDifficultyId,
            };

            //Convert Domain back to DTO
            return Ok(walkDTO);
        }
    }
}
