using Microsoft.AspNetCore.Mvc;
using UdemyProject.Models.Domain;

namespace UdemyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {

        private readonly Repositories.IImageRepository _imageRepository;
        public ImagesController(Repositories.IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAll()
        {
            IEnumerable<ImageDomain> imageDomain = await _imageRepository.DownloadAll();
            return Ok(imageDomain);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> DownloadById(Guid id)
        {
            ImageDomain imageDomain = await _imageRepository.DownloadById(id);

            if (imageDomain == null)
            {
                return NotFound();
            }

            return Ok(imageDomain);
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] Models.DTO.ImageDTO imageDTO) 
        {

            ValidateFile(imageDTO);
            
            if (ModelState.IsValid)
            {

                var imageDomain = new Models.Domain.ImageDomain
                {
                    File = imageDTO.File,
                    FileExtension = Path.GetExtension(imageDTO.File.FileName),
                    FileSizeInBytes = imageDTO.File.Length,
                    FileName = imageDTO.FileName,
                    FileDescription = imageDTO.FileDescription
                };

                imageDomain = await _imageRepository.Upload(imageDomain);
                return Ok(imageDomain);
            }
            {
                return BadRequest(ModelState);
            } 
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] Models.DTO.ImageDTO imageDTO)
        {
            ValidateFile(imageDTO);

            if (ModelState.IsValid)
            {
                var imageDomain = new Models.Domain.ImageDomain
                {
                    File = imageDTO.File,
                    FileExtension = Path.GetExtension(imageDTO.File.FileName),
                    FileSizeInBytes = imageDTO.File.Length,
                    FileName = imageDTO.FileName,
                    FileDescription = imageDTO.FileDescription
                };

                imageDomain = await _imageRepository.Update(id, imageDomain);

                if (imageDomain == null)
                {
                    return NotFound();
                }

                return Ok(imageDomain);
            }
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete (Guid id)
        {
            ImageDomain imageDomain = await _imageRepository.Delete(id);

            if (imageDomain == null)
            {
                return NotFound();
            }

            return Ok(imageDomain);
        }


        #region File Upload Validation
        private void ValidateFile (Models.DTO.ImageDTO imageDTO)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png"};

            if (!allowedExtension.Contains(Path.GetExtension(imageDTO.File.FileName)))
            {
                ModelState.AddModelError("File", "Unsupported file extension");
            }

            if (imageDTO.File.Length > 10485760) //bytes value 
            {
                ModelState.AddModelError("File", "File size more than 10 mb. Please upload a smaller size file");
            }
        } 

        #endregion
    }
}