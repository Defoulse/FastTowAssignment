using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FastTowAssignment.Models
{
    public class BlobUtility
    {
        public CloudStorageAccount storageAccount;
        public BlobUtility()
        {
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName=blobstorage053187;AccountKey=Sn+l3khCLrdLjUh9SBDy5rYq1j/unhQFdOJV/L1ca2L0UjKUIhHlT27GyLNooE+1F6S9gzFEKzIoUHuYbSlqCw==;EndpointSuffix=core.windows.net", "blobstorage053187", "Sn+l3khCLrdLjUh9SBDy5rYq1j/unhQFdOJV/L1ca2L0UjKUIhHlT27GyLNooE+1F6S9gzFEKzIoUHuYbSlqCw==");
            storageAccount = CloudStorageAccount.Parse(UserConnectionString);
        }

        public async Task<CloudBlockBlob> UploadBlobAsync(string BlobName, string ContainerName, IFormFile file)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName.ToLower());
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);

            try
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    await blockBlob.UploadFromStreamAsync(ms);
                }
                return blockBlob;
            }
            catch (Exception e)
            {
                var r = e.Message;
                return null;
            }
        }

        public void DeleteBlob(string BlobName, string ContainerName)
        {

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            blockBlob.DeleteAsync();
        }

        public CloudBlockBlob DownloadBlob(string BlobName, string ContainerName)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            return blockBlob;
        }
    }
}
