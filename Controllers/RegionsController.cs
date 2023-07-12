using Microsoft.AspNetCore.Mvc;
using UdemyProject.Models.Domain;
using UdemyProject.Repositories;
using Microsoft.AspNetCore.Authorization;
  
namespace UdemyProject.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly ILogger<RegionsController> _logger;
        public RegionsController(IRegionRepository regionRepository, ILogger<RegionsController> logger)
        {
            _regionRepository = regionRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            try
            {
                _logger.LogInformation("GetAllRegionsAsync is executed");
                IEnumerable<RegionDomain> regionDomain = await _regionRepository.GetAllAsync();
                return Ok(regionDomain);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
        }

        [HttpGet]
        [Route("{id:guid}")] 
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("GetRegionsAsync is executed");
                RegionDomain? regionDomain = await _regionRepository.GetAsync(id);
                
                if (regionDomain == null)
                {
                    return NotFound();
                }

                return Ok(regionDomain);
            }
            catch (System.Exception e)
            {
                 _logger.LogError(e, e.Message);
                return BadRequest(e);
            }   
        }

        [HttpPost]
        [Authorize(Roles = "writer")] 
        public async Task<IActionResult> AddRegionAsync(Models.DTO.RegionDTO regionDTO)
        {

            try
            {
                _logger.LogInformation("AddRegionAsync is executed");
                RegionDomain? regionDomain = new RegionDomain()
                {
                    Code = regionDTO.Code,
                    Area = regionDTO.Area,
                    Lat = regionDTO.Lat,
                    Long = regionDTO.Long,
                    Name = regionDTO.Name,
                    Population = regionDTO.Population
                }; 

                regionDomain = await _regionRepository.AddAsync(regionDomain);
                return Ok(regionDomain);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")] 
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.RegionDTO regionDTO)
        {
            try
            {
                 _logger.LogInformation("UpdateRegionAsync is executed");

                 RegionDomain? regionDomain = new RegionDomain()
                {
                    Code = regionDTO.Code,
                    Area = regionDTO.Area,
                    Lat = regionDTO.Lat,
                    Long = regionDTO.Long,
                    Name = regionDTO.Name,
                    Population = regionDTO.Population,
                };

                regionDomain = await _regionRepository.UpdateAsync(id, regionDomain);

                if (regionDomain == null)
                {
                    return NotFound();
                }

                return Ok(regionDomain);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")] 
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("DeleteRegionAsync is executed");
                RegionDomain? regionDomain = await _regionRepository.DeleteAsync(id);

                if (regionDomain == null)
                {
                    return NotFound();
                }

                return Ok(regionDomain);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e);
            }
        }
    }
}
