using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FastTowAssignment.Controllers
{
    public class BlobsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private CloudBlobContainer getContainerInformation()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build();

            CloudStorageAccount accountDetails = CloudStorageAccount.Parse(configure["ConnectionStrings:BlobStorageConnection"]);
            CloudBlobClient clientAgent = accountDetails.CreateCloudBlobClient();
            CloudBlobContainer container = clientAgent.GetContainerReference("testblob");

            return container;
        }

        public IActionResult CreateContainerPage()
        {
            CloudBlobContainer container = getContainerInformation();
            ViewBag.result = container.CreateIfNotExistsAsync().Result;
            ViewBag.containerName = container.Name;
            return View();
        }

        public string uploadTextFile()
        {
            CloudBlobContainer container = getContainerInformation();
            CloudBlockBlob blobItem = container.GetBlockBlobReference("MyTextFile.txt");

            try
            {
                var fileStream = System.IO.File.OpenRead(@"C:\\Users\\Defoulse\\Desktop\\Bro.txt");
                using (fileStream)
                {
                    blobItem.UploadFromStreamAsync(fileStream).Wait();
                }
            }
            catch(Exception ex)
            {
                return "Technical issue: " + ex.ToString() + ". Please upload the file again!";
            }

            return blobItem.Name + "is success uploaded to the blob storage account URL: " + blobItem.Uri;
        }
    }
}
