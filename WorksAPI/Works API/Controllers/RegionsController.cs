Effici
﻿using Microsoft.AspNetCore.Mvc;
using Works_API.Models.Domain;
using Works_API.Repositories;
using Works_API.Models.DTO;
using AutoMapper;

namespace Works_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController (IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            
            var regions = await regionRepository.GetAllAsync();


            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return  NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // Request(DTO) to Domain model

            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population,
            };

            // Pass details to Repositpory
            region = await regionRepository.AddAsync(region);

            // Convert back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get a region from database
            var region = await regionRepository.DeleteAsync(id);


            //if null NotFound
            if (region == null)
            {
                return NotFound();
            }
            //Convert response back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };
            //return Ok response
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest UpdateRegionRequest)
        {
            //Convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = UpdateRegionRequest.Code,
                Name = UpdateRegionRequest.Name,
                Area = UpdateRegionRequest.Area,
                Lat = UpdateRegionRequest.Lat,
                Long = UpdateRegionRequest.Long,
                Population = UpdateRegionRequest.Population,
            };

            //Updatre Region Usijng Repository
            var regionUpdate = await regionRepository.UpdateAsync(id, region);

            //If  null thyen Not Found
            if(regionUpdate == null)
            {
                return NotFound();
            }
            var regionDTO = new Models.DTO.Region()
            {
                Id = regionUpdate.Id,
                Code = regionUpdate.Code,
                Area = regionUpdate.Area,
                Lat = regionUpdate.Lat,
                Long = regionUpdate.Long,
                Name = regionUpdate.Name,
                Population = regionUpdate.Population,
            };

            //Convert Domain back to DTO
            return Ok(regionDTO);
        }
    }
}


            
