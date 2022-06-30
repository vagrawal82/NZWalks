using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
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
            var walksDomain = await walkRepository.GetAllAsync();

            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walkDomain = await walkRepository.GetAsync(id);

            var walksDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walksDTO);
        }

        [HttpPost]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
        {
            //Validate the request
            if (!(await ValidateAdWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };

            walkDomain = await walkRepository.AddAsync(walkDomain);

            var walkDTO = new Models.DTO.Walk
            {
                Id =   walkDomain.Id,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId=walkDomain.WalkDifficultyId,
            };

            return CreatedAtAction(nameof(GetWalkAsync), new {id = walkDTO.Id} , walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, 
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Validate the request
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            walkDomain =await walkRepository.UpdateAsync(id, walkDomain);

            if(walkDomain == null)
            {
                return NotFound("Walk not found");
            }
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };
            return Ok(walkDTO);       
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //get region from DB
            var walk = await walkRepository.DeleteAsync(id);

            //if null then return not found
            if (walk == null)
            {
                return NotFound();
            }
            //else convert response back to DTO           
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            //return ok respose
            return Ok(walkDTO);
        }

        #region Private Methods

        private async Task<bool> ValidateAdWalkAsync(AddWalkRequest addWalkRequest)
        {
            //if(addWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest),
            //        $"Add walk request data is required.");
            //    return false;
            //}
            
            //if(string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name),
            //        $"{nameof(addWalkRequest.Name)} cannot be null or empty or white space.");
            //}

            //if (addWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length),
            //        $"{nameof(addWalkRequest.Length)} cannot be less than or erual to 0.");
            //}

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                   $"{nameof(addWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty =await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                   $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(UpdateWalkRequest updateWalkRequest)
        {

            //if (updateWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest),
            //        $"Add walk request data is required.");
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Name),
            //        $"{nameof(updateWalkRequest.Name)} cannot be null or empty or white space.");
            //}

            //if (updateWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Length),
            //        $"{nameof(updateWalkRequest.Length)} cannot be less than or erual to 0.");
            //}

            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                   $"{nameof(updateWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                   $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");
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
