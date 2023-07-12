
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyProject.Models.Domain;

namespace UdemyProject.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Data.DBContextClass _dbContextClass;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, Data.DBContextClass dBContextClass)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContextClass = dBContextClass;
        }

        public async Task<ImageDomain> Delete(Guid id)
        {
            ImageDomain? imageDomain = await _dbContextClass.ImagesTable.FirstOrDefaultAsync(x => x.Id == id);

            if (imageDomain != null)
            {
                _dbContextClass.ImagesTable.Remove(imageDomain);
                await _dbContextClass.SaveChangesAsync();
            }

            return imageDomain;
        }

        public async Task<IEnumerable<ImageDomain>> DownloadAll()
        {
            return await _dbContextClass.ImagesTable.ToListAsync();
        }

        public async Task<ImageDomain> DownloadById(Guid id)
        {
            return await _dbContextClass.ImagesTable.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ImageDomain> Update(Guid id, Models.Domain.ImageDomain imageDomain)
        {
            ImageDomain? existingImage = await _dbContextClass.ImagesTable.FirstOrDefaultAsync(x => x.Id == id);

            if (existingImage != null) 
            {
                existingImage.File = imageDomain.File;
                existingImage.FileName = imageDomain.FileName;
                existingImage.FileDescription = imageDomain.FileDescription;
                existingImage.FileExtension = imageDomain.FileExtension;
                existingImage.FileSizeInBytes = imageDomain.FileSizeInBytes;
            }
            
            existingImage.FilePath = CreateFilePath(imageDomain);

            await _dbContextClass.SaveChangesAsync();
            return existingImage;
           
        }

        public async Task<ImageDomain> Upload (ImageDomain imageDomain) 
        {

            imageDomain.FilePath = CreateFilePath(imageDomain);

            await _dbContextClass.ImagesTable.AddAsync(imageDomain);
            await _dbContextClass.SaveChangesAsync();

            return imageDomain;
        }


        private string CreateFilePath (ImageDomain imageDomain)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{imageDomain.FileName}{imageDomain.FileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            imageDomain.File.CopyToAsync(stream);

            string? urlFilePath = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}{_httpContextAccessor.HttpContext?.Request.PathBase}/Images/{imageDomain.FileName}{imageDomain.FileExtension}";

            return urlFilePath;
        }
    }

}