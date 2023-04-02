using Microsoft.AspNetCore.Mvc;
using UdemyProject.Models.Domain;
using UdemyProject.Models.DTO;
using UdemyProject.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace UdemyProject.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _imapper;
        public RegionsController(IRegionRepository regionRepository, IMapper imapper)
        {
            _regionRepository = regionRepository;
            _imapper = imapper;
        }

        [HttpGet]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetAllRegionsAsync()
        {

            IEnumerable<RegionDomain> regionDomain = await _regionRepository.GetAllAsync();
            // get the IEnumerable of RegionDomain from the GetAllAsync method in the RegionRepository

            IEnumerable<RegionDTO> regionDTO = _imapper.Map<List<Models.DTO.RegionDTO>>(regionDomain);
            // get the IEnumerable of RegionDTO from the converted regionDomain object

            return Ok(regionDTO);
            // return the regionDTO IEnumerable/List
        }

        [HttpGet]
        [Route("{id:guid}")] /// create space to insert the ID and validate the inserted ID
        [ActionName("GetRegion")]
        [Authorize(Roles = "reader,writer")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            RegionDomain? regionDomain = await _regionRepository.GetAsync(id);
            // get the single object of regionDomain from the GetAsync method in the RegionRepository
            // GetAsync method carry forward Id value inserted by the user in swagger UI

            if (regionDomain == null)
            {
                return NotFound();
            }

            RegionDTO? regionDTO = _imapper.Map<Models.DTO.RegionDTO>(regionDomain);
            // if the regionDomain is not null, then the Imapper will map the regionDomain object to the regionDTO object

            return Ok(regionDTO);
            // return the regionDTO object
        }

        [HttpPost]
        [Authorize(Roles = "writer")] 
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {

            RegionDomain? regionDomain = new RegionDomain()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            }; 
            // new object of RegionDomain? regionDomain carry the values inserted by the user in Swagger UI based on the AddRegionRequest

            regionDomain = await _regionRepository.AddAsync(regionDomain);
            // regionDomain stores the value of the saved regionDomain that was used in the AddAsync method

            RegionDTO? regionDTO = new RegionDTO()
            {
                RegionId = regionDomain.Id,
                RegionCode = regionDomain.Code,
                RegionArea = regionDomain.Area,
                RegionLat = regionDomain.Lat,
                RegionLong = regionDomain.Long,
                RegionName = regionDomain.Name,
                RegionPopulation = regionDomain.Population,
            };
            // Map the regionDomain data to the regionDTO data
            // RegionDomain.Id was created in the RegionRepository

            return CreatedAtAction("GetRegion", new { id = regionDTO.RegionId }, regionDTO);
            // ControllerBase.CreatedAtAction Method: CreatedAtAction(String, Object, Object)
            // string = action name found in get method
            // object = unique route values (Id of to be created object)
            // object = content value of the entity body (we use regionDTO)

        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")] 
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {

            RegionDomain? regionDomain = await _regionRepository.DeleteAsync(id);
            // RegionDomain? regionDomain stores the value get from the DeleteAsync method

            if (regionDomain == null)
                // regionDomain is not null, instead it only empty
            {
                return NotFound();
            }

            RegionDTO? regionDTO = new RegionDTO()
            {
                RegionId = regionDomain.Id,
                RegionCode = regionDomain.Code,
                RegionArea = regionDomain.Area,
                RegionLat = regionDomain.Lat,
                RegionLong = regionDomain.Long,
                RegionName = regionDomain.Name,
                RegionPopulation = regionDomain.Population,
            };
            // Map the regionDomain data to the regionDTO data

            return Ok(regionDTO);
            // return the already remove data. The response will be blank as it no longer exists
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")] 
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            
            RegionDomain? regionDomain = new Models.Domain.RegionDomain()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };
            // new object of RegionDomain? regionDomain carry the values inserted by the user in Swagger UI based on the AddRegionRequest

            regionDomain = await _regionRepository.UpdateAsync(id, regionDomain);
            // regionDomain stores the value of the saved regionDomain that was used in the UpdateAsync method

            if (regionDomain == null)
            {
                return NotFound();
            }
            // To check if there is a user data where the Id is matched with what inserted by the user in the Swagger UI

            RegionDTO? regionDTO = new Models.DTO.RegionDTO()
            {
                RegionId = regionDomain.Id,
                RegionCode = regionDomain.Code,
                RegionArea = regionDomain.Area,
                RegionLat = regionDomain.Lat,
                RegionLong = regionDomain.Long,
                RegionName = regionDomain.Name,
                RegionPopulation = regionDomain.Population,
            };
            // // Map the regionDomain data to the regionDTO data

            return Ok(regionDTO);
            // returned the updated data to user
        }

    }
}
