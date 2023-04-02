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
    public class DifficultyController : Controller
    {
        private readonly IDifficultyRepository _difficultyRepository;
        private readonly IMapper _mapper;
        public DifficultyController(IDifficultyRepository difficultyRepository, IMapper mapper)
        {
            _difficultyRepository = difficultyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetAllDifficulty()
        {
            IEnumerable<DifficultyDomain> difficulty = await _difficultyRepository.GetAllAsync();

            IEnumerable<DifficultyDTO> difficultyDTO = _mapper.Map<List<Models.DTO.DifficultyDTO>>(difficulty);

            return Ok(difficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficulty")]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetDifficultyAsync(Guid id)
        {
            DifficultyDomain? difficulty = await _difficultyRepository.GetAsync(id);

            if(difficulty == null)
            {
                return NotFound();
            }

            DifficultyDTO walkdifficultyDTO = _mapper.Map<Models.DTO.DifficultyDTO>(difficulty);

            return Ok(walkdifficultyDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddDifficulty(Models.DTO.AddDifficultyRequest addDifficultyRequest)
        {

            DifficultyDomain? difficulty = new Models.Domain.DifficultyDomain
            {
                Code = addDifficultyRequest.Code,
            };

            difficulty = await _difficultyRepository.AddAsync(difficulty);

            DifficultyDTO? difficultyDTO = _mapper.Map<Models.DTO.DifficultyDTO>(difficulty);

            return CreatedAtAction("GetWalkDifficulty", new { id = difficultyDTO.DifficultyId }, difficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateDifficulty(Guid id, Models.DTO.UpdateDifficultyRequest updateDifficultyRequest)
        {
            
            DifficultyDomain? difficulty = new Models.Domain.DifficultyDomain
            { 
                Code = updateDifficultyRequest.Code
            };

            difficulty = await _difficultyRepository.UpdateAsync(id, difficulty);

            if (difficulty == null)
            {
                return NotFound();
            }
            
            DifficultyDTO? difficultyDTO = _mapper.Map<Models.DTO.DifficultyDTO>(difficulty);

            return Ok(difficultyDTO);
        }

        [HttpDelete] 
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            DifficultyDomain? difficulty = await _difficultyRepository.DeleteAsync(id);

            if (difficulty == null)
            {
                return NotFound();
            }

            DifficultyDTO? difficultyDTO = _mapper.Map<Models.DTO.DifficultyDTO>(difficulty);
            return Ok(difficultyDTO);
        }
    }
}
