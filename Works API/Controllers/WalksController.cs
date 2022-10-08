using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Works_API.Repositories;
using Works_API.Models.DTO;
using Works_API.Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Works_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalksAsync()
        {

            var walks = await walkRepository.GetAllAsync();


            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            // Convert DTO to Domain Object

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
            var walkDTO = new Models.DTO.Walk()
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
        [Authorize(Roles = "writer")]
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
                //or use var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            };
            //return Ok response
            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest UpdateWalkRequest)
        {
            if (!(await ValidateUpdateWalkAsync(UpdateWalkRequest)))
            {
                return BadRequest(ModelState);
            }
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

        #region Private Methods


        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} Add walk data is required, hence data cannot be empty");
                return false;

            }

            //if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} Ensure the name field is not empty");
            //}

            //if (addWalkRequest.Length < 0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} The Length cannot be zero or negative value");
            //}

             var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} RegionId is invalid");
            }

            var walkdifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} WalkDifficultyId is invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest UpdateWalkRequest)
        {
            if (UpdateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(UpdateWalkRequest), $"{nameof(UpdateWalkRequest)} Add walk data is required, hence data cannot be empty");
                return false;

            }

            //if (string.IsNullOrWhiteSpace(UpdateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(UpdateWalkRequest.Name), $"{nameof(UpdateWalkRequest.Name)} Ensure the name field is not empty");
            //}

            //if (UpdateWalkRequest.Length < 0)
            //{
            //    ModelState.AddModelError(nameof(UpdateWalkRequest.Length), $"{nameof(UpdateWalkRequest.Length)} The Length cannot be zero or negative value");
            //}

            var region = await regionRepository.GetAsync(UpdateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(UpdateWalkRequest.RegionId), $"{nameof(UpdateWalkRequest.RegionId)} RegionId is invalid");
            }

            var walkdifficulty = await walkDifficultyRepository.GetAsync(UpdateWalkRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(UpdateWalkRequest.WalkDifficultyId), $"{nameof(UpdateWalkRequest.WalkDifficultyId)} WalkDifficultyId is invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }


        #endregion
    }
}
