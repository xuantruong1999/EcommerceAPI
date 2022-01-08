using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services
{
    public interface IBlobStorageAccountService
    {
        Task<string> UploadFileToBlob(IFormFile file);
        void DeleteBlobData(string fileUrl);
    }
    public class BlobStorageAccountService : IBlobStorageAccountService
    {
        private readonly IConfiguration _configuration;
        private string _accessKey;
        private string _accountName => new string("eco99web");
        private string _container => new string("images");
        private string _connectStringStorage;
        public BlobStorageAccountService(IConfiguration configuation)
        {
            _configuration = configuation;
            _accessKey = _configuration.GetSection("ConnectionStringBlobStorage:AccessKey").Value;
            _connectStringStorage = _configuration.GetSection("ConnectionStringBlobStorage:ConnectionString").Value;
        }

        public async Task<string> UploadFileToBlob(IFormFile file)
        {
            try
            {
                string fileName = GenerateProductImageFileName(file.FileName);
                // Create a URI to the blob
                Uri blobUri = new Uri("https://" +
                                      _accountName +
                                      ".blob.core.windows.net/" +
                                      _container +
                                      "/" + fileName);

                StorageSharedKeyCredential storageCredentials =
                        new StorageSharedKeyCredential(_accountName, _accessKey);

                BlobClient blobClient = new BlobClient(blobUri, storageCredentials);
                await blobClient.UploadAsync(file.OpenReadStream());
                return blobUri.AbsoluteUri;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async void DeleteBlobData(string fileUrl)
        {
            Uri uriObj = new Uri(fileUrl);
            var parseblobName = uriObj.AbsolutePath.Split("/");
            var blobName = parseblobName[2] + "/" + parseblobName[3];
            BlobClient blobClient = new BlobClient(_connectStringStorage, _container, blobName);
            var result = await blobClient.DeleteIfExistsAsync();
        }

        private string GenerateProductImageFileName(string strFileName) => "products/" + Guid.NewGuid().ToString() + strFileName;
        
    }
}
