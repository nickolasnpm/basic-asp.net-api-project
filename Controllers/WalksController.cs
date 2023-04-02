using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UdemyProject.Models.Domain;
using UdemyProject.Models.DTO;
using UdemyProject.Repositories;

namespace UdemyProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        private readonly IRegionRepository _regionRepository; 
        private readonly IDifficultyRepository _difficultyRepository;
        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IDifficultyRepository difficultyRepository)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
            _regionRepository = regionRepository;
            _difficultyRepository = difficultyRepository;
        }

        [HttpGet]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            IEnumerable<Models.Domain.WalkDomain> walkDomain = await _walkRepository.GetAllAsync();

            IEnumerable<Models.DTO.WalkDTO> walksDTO = _mapper.Map<List<Models.DTO.WalkDTO>>(walkDomain);

            return Ok(walksDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalks")]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetWalksAsync(Guid id)
        {
            WalkDomain? walkDomain = await _walkRepository.GetAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            WalkDTO? walksDTO = _mapper.Map<Models.DTO.WalkDTO>(walkDomain);

            return Ok(walksDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {

            if (!(await ValidateAddWalkAsync (addWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            // Additional to AddWalkRequestValidator. Required this validation to complement limited capacity of Fluent Validation

            WalkDomain? walkDomain = new Models.Domain.WalkDomain
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                DifficultyId = addWalkRequest.DifficultyId,
            };

            walkDomain = await _walkRepository.AddAsync(walkDomain);

            WalkDTO WalkDTO = new Models.DTO.WalkDTO
            {
                WalkId = walkDomain.Id,
                WalkName = walkDomain.Name,
                WalkLength = walkDomain.Length,
                WalkRegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.DifficultyId,
            };

            return CreatedAtAction("GetWalks", new { Id = WalkDTO.WalkId }, WalkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest))) 
            { 
                return BadRequest(ModelState); 
            }

            WalkDomain? walkDomain = new Models.Domain.WalkDomain
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                DifficultyId = updateWalkRequest.DifficultyId,
            };

           walkDomain = await _walkRepository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
            {
                return NotFound();
            }

            WalkDTO? walkDTO = new Models.DTO.WalkDTO
            {
                WalkId = walkDomain.Id,
                WalkName = walkDomain.Name,
                WalkLength = walkDomain.Length,
                WalkRegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.DifficultyId,
            };

            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            WalkDomain? walkDomain = await _walkRepository.DeleteAsync(id);

            if (walkDomain == null) 
            { 
                return NotFound(); 
            }

            WalkDTO? walkDTO = _mapper.Map<Models.DTO.WalkDTO>(walkDomain);

            return Ok(walkDTO);
        }

        #region Private : Data Validation

        private async Task<bool> ValidateAddWalkAsync (Models.DTO.AddWalkRequest addWalkRequest)
        {    

            RegionDomain? region = await _regionRepository.GetAsync(addWalkRequest.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is Invalid");
            }

            DifficultyDomain? difficulty = await _difficultyRepository.GetAsync(addWalkRequest.DifficultyId);

            if (difficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.DifficultyId), $"{nameof(addWalkRequest.DifficultyId)} is Invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private async Task<bool> ValidateUpdateWalkAsync (Models.DTO.UpdateWalkRequest updateWalkRequest)
        {

            RegionDomain? region = await _regionRepository.GetAsync(updateWalkRequest.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is Invalid");
            }

            DifficultyDomain? difficulty = await _difficultyRepository.GetAsync(updateWalkRequest.DifficultyId);

            if (difficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.DifficultyId), $"{nameof(updateWalkRequest.DifficultyId)} is Invalid");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}