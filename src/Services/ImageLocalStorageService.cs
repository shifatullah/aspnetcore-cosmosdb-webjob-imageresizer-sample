namespace Images.Services
{
    using Images.Entities;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImageLocalStorageService : IImageStorageService
    {
        IWebHostEnvironment _hostEnvironment;

        public ImageLocalStorageService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<bool> UploadImage(Stream stream, string fileName)
        {
            // save image to wwwroot/image
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/images/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {                
                await stream.CopyToAsync(fileStream);
            }

            return await Task.FromResult<bool>(true);
        }

        public async Task<bool> DeleteImage(string fileName)
        {
            // delete image from wwwroot/image                
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            return await Task.FromResult<bool>(true);
        }

        public string GetImageUrl(string fileName)
        {
            return $"~/images/{fileName}";
        }
    }
}