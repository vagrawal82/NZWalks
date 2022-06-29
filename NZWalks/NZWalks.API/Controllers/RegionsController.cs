using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;

        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();
            //return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,                   
            //    };
            //    regionsDTO.Add(regionDTO);
            //});
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
                return NotFound();
            }
            var regionsDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionsDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest addRegionRequest)
        {
            //Request to domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population,
            };
            //Pass details to repositry
            region = await regionRepository.AddAsync(region);

            //Convert back to DTO
            var regionsDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionsDTO.Id },regionsDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //get region from DB
            var region = await regionRepository.DeleteAsync(id);

            //if null then return not found
            if (region == null)
            {
                return NotFound();
            }
            //else convert response back to DTO           
            var regionsDTO = mapper.Map<Models.DTO.Region>(region);

            //return ok respose
            return Ok(regionsDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequest updateRegionRequest)
        {
            //convert dto to domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name= updateRegionRequest.Name,
                Lat = updateRegionRequest.Lat,  
                Long= updateRegionRequest.Long,
                Population= updateRegionRequest.Population,
                Area = updateRegionRequest.Area,
                               
            };

            //Update region using repo
            region  = await regionRepository.UpdateAsync(id, region);

            //if null then not found
            if(region == null)
            {
                return NotFound();
            }

            //convert domain back to dto
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            //return ok 
            return Ok(regionDTO);
        }
    }
}
