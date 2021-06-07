using Images.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Images.Services
{

    public interface IImageStorageService
    {
        Task<bool> UploadImage(Stream stream, string fileName);
        Task<bool> DeleteImage(string fileName);

        string GetImageUrl(string fileName);

        string GetImageUrl(string containerName, string fileName);
    }
}
