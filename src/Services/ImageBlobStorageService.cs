namespace Images.Services
{
    using Azure.Storage;
    using Azure.Storage.Blobs;
    using Images.Entities;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImageBlobStorageService : IImageStorageService
    {       
        private readonly AzureStorageConfig _storageConfig = null;

        public ImageBlobStorageService(IOptions<AzureStorageConfig> options)
        {
            _storageConfig = options.Value;
        }

        public async Task<bool> DeleteImage(string fileName)
        {
            Uri blobUri = new Uri(GetImageUrl(fileName));

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.Account, _storageConfig.Key);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

            // Upload the file
            await blobClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);

            // delete thumbnail
            Uri thumbnailUri = new Uri(GetImageUrl("thumbnails", fileName));            
            BlobClient thumbnailClient = new BlobClient(thumbnailUri, storageCredentials);
            await thumbnailClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);

            return await Task.FromResult(true);
        }

        public string GetImageUrl(string fileName)
        {
            return GetImageUrl(_storageConfig.ImageContainerName, fileName);
        }

        public string GetImageUrl(string containerName, string fileName)
        {
            return $"https://{_storageConfig.Account}.blob.core.windows.net/{containerName}/{fileName}";
        }

        public async Task<bool> UploadImage(Stream fileStream, string fileName)
        {
            Uri blobUri = new Uri(GetImageUrl(fileName));

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.Account, _storageConfig.Key);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);            

            // Upload the file
            await blobClient.UploadAsync(fileStream);

            return await Task.FromResult(true);
        }
    }
}