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
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName=fasttowassignmentstorage;AccountKey=RUebNyt3PV32RJrLj9rQJ5OoHm55vt9V6qWFxXVk6d5ctQsacmveXawIkSBvOGSfr5kKWv6Y9fc5xnsTnLVfBQ==;EndpointSuffix=core.windows.net");
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
