using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using UdemyProject.Models.Domain;
using UdemyProject.Models.DTO;
using UdemyProject.Repositories;

namespace UdemyProject.Controllers
{ 
    [ApiController]
    [Route("[controller]")]
    public class DifficultyController : Controller
    {
        private readonly IDifficultyRepository _difficultyRepository;
        private readonly ILogger<DifficultyController> _logger;
        public DifficultyController(IDifficultyRepository difficultyRepository, ILogger<DifficultyController> logger)
        {
            _difficultyRepository = difficultyRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetAllDifficulty()
        {
            try
            {
                _logger.LogInformation("GetAllDifficulty is executed");
                IEnumerable<DifficultyDomain> difficulty = await _difficultyRepository.GetAllAsync();
                return Ok(difficulty);
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
        public async Task<IActionResult> GetDifficultyAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("GetDifficultyAsync is executed");

                DifficultyDomain? difficulty = await _difficultyRepository.GetAsync(id);

                if (difficulty == null)
                {
                    return NotFound();
                }

                return Ok(difficulty);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
            
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddDifficulty(DifficultyDTO difficultyDTO)
        {
            try
            {
                _logger.LogInformation("Addifficulty is executed");

                DifficultyDomain? difficulty = new DifficultyDomain
                {
                    Code = difficultyDTO.Code,
                };

                difficulty = await _difficultyRepository.AddAsync(difficulty);
                return Ok(difficulty);
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
        public async Task<IActionResult> UpdateDifficulty(Guid id, DifficultyDTO difficultyDTO)
        {
            try
            {
                _logger.LogInformation("UpdateDifficulty is executed");

                DifficultyDomain? difficulty = new DifficultyDomain
                {
                    Code = difficultyDTO.Code
                };

                difficulty = await _difficultyRepository.UpdateAsync(id, difficulty);

                if (difficulty == null)
                {
                    return NotFound();
                }

                return Ok(difficulty);
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
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            try
            {
                _logger.LogInformation("DeleteWalkDifficulty is executed");

                DifficultyDomain? difficulty = await _difficultyRepository.DeleteAsync(id);

                if (difficulty == null)
                {
                    return NotFound();
                }

                return Ok(difficulty);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
        }
    }
}
