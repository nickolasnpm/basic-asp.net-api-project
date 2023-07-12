using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyProject.Models.Domain;
using UdemyProject.Repositories;

namespace UdemyProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IRegionRepository _regionRepository; 
        private readonly IDifficultyRepository _difficultyRepository;
        private readonly ILogger<DifficultyController> _logger;
        public WalksController(IWalkRepository walkRepository, IRegionRepository regionRepository, IDifficultyRepository difficultyRepository, ILogger<DifficultyController> logger)
        {
            _walkRepository = walkRepository;
            _regionRepository = regionRepository;
            _difficultyRepository = difficultyRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            try
            {
                _logger.LogInformation("GetAllWalksAsync is executed");

                IEnumerable<Models.Domain.WalkDomain> walkDomain = await _walkRepository.GetAllAsync();
                return Ok(walkDomain);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
           
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetWalksAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("GetWalksAsync is executed");

                WalkDomain? walkDomain = await _walkRepository.GetAsync(id);

                if (walkDomain == null)
                {
                    return NotFound();
                }

                return Ok(walkDomain);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.WalkDTO walkDTO)
        {
            try
            {
                _logger.LogInformation("AddWalkAsync is executed");

                if (!(await ValidateAddWalkAsync(walkDTO)))
                {
                    return BadRequest(ModelState);
                }

                WalkDomain? walkDomain = new Models.Domain.WalkDomain
                {
                    Name = walkDTO.Name,
                    Length = walkDTO.Length,
                    RegionId = walkDTO.RegionId,
                    DifficultyId = walkDTO.DifficultyId,
                };

                walkDomain = await _walkRepository.AddAsync(walkDomain);

                return Ok(walkDomain);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.WalkDTO walkDTO)
        {
            try
            {
                _logger.LogInformation("UpdateWalkAsync is executed");

                if (!(await ValidateUpdateWalkAsync(walkDTO)))
                {
                    return BadRequest(ModelState);
                }

                WalkDomain? walkDomain = new Models.Domain.WalkDomain
                {
                    Name = walkDTO.Name,
                    Length = walkDTO.Length,
                    RegionId = walkDTO.RegionId,
                    DifficultyId = walkDTO.DifficultyId,
                };

                walkDomain = await _walkRepository.UpdateAsync(id, walkDomain);

                if (walkDomain == null)
                {
                    return NotFound();
                }

                return Ok(walkDomain);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("DeleteWalkAsync is executed");

                WalkDomain? walkDomain = await _walkRepository.DeleteAsync(id);

                if (walkDomain == null)
                {
                    return NotFound();
                }

                return Ok(walkDomain);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
            
        }

        #region Private : Data Validation

        private async Task<bool> ValidateAddWalkAsync (Models.DTO.WalkDTO walkDTO)
        {    

            RegionDomain? region = await _regionRepository.GetAsync(walkDTO.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(walkDTO.RegionId), $"{nameof(walkDTO.RegionId)} is Invalid");
            }

            DifficultyDomain? difficulty = await _difficultyRepository.GetAsync(walkDTO.DifficultyId);

            if (difficulty == null)
            {
                ModelState.AddModelError(nameof(walkDTO.DifficultyId), $"{nameof(walkDTO.DifficultyId)} is Invalid");
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

        private async Task<bool> ValidateUpdateWalkAsync (Models.DTO.WalkDTO walkDTO)
        {

            RegionDomain? region = await _regionRepository.GetAsync(walkDTO.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(walkDTO.RegionId), $"{nameof(walkDTO.RegionId)} is Invalid");
            }

            DifficultyDomain? difficulty = await _difficultyRepository.GetAsync(walkDTO.DifficultyId);

            if (difficulty == null)
            {
                ModelState.AddModelError(nameof(walkDTO.DifficultyId), $"{nameof(walkDTO.DifficultyId)} is Invalid");
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