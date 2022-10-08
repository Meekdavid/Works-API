using Microsoft.AspNetCore.Mvc;
using Works_API.Repositories;
using AutoMapper;
using Works_API.Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Works_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        public readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository , IMapper mapper)
        {
           this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var mbokoDomain = await walkDifficultyRepository.GetAllAsync();
            var mbokoDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(mbokoDomain);
            return Ok(mbokoDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
           var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest Mboko)
        {
            if (!ValidateAddWalkDifficultyAsync(Mboko))
            {
                return BadRequest(ModelState);
            }
            // Convert DTO to Domain Model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = Mboko.Code,
            };

            //Call Repository
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //Convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);
            return CreatedAtAction(nameof(GetWalkDifficultyById), new {id = walkDifficultyDTO.Id}, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id, Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
            // Convert DTO to Domain Model
            var walkDifficultyDomainN = new WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //Call repository to update
            walkDifficultyDomainN = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomainN);
            if (walkDifficultyDomainN == null)
            {
                return NotFound();
            }
            //Convert Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomainN);

            //Return response
            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            //Call Repository to dekete
            var waldifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);
            if (waldifficultyDomain == null)
            {
                return NotFound();
            }

            //Convert Domain back to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(waldifficultyDomain);

            //Return response\
            return Ok(walkDifficultyDTO);
        }


        #region Private Methods


        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest Mboko)
        {
            if (Mboko == null)
            {
                ModelState.AddModelError(nameof(Mboko), $"{nameof(Mboko)} The Add Walk Difficulty data cannot be empty.");
                return false;

            }

            if (string.IsNullOrWhiteSpace(Mboko.Code))
            {
                ModelState.AddModelError(nameof(Mboko.Code), $"{nameof(Mboko.Code)} Cannot be null or empty or white space");
            }

           
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"{nameof(updateWalkDifficultyRequest)} Add walk Difficulty data is required.");
                return false;

            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} Cannot be null or empty or whoite space");
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
