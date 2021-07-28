using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        //public string uploadMultipleBlob()
        //{
        //    CloudBlobContainer container = getContainerInformation();

        //    string name = ""; string uploadedFileSentence = "";

        //    for (int i = 1; i <= 3; i++)
        //    {
        //        CloudBlockBlob blobItem = container.GetBlockBlobReference("image" + i + ".jpg");

        //        try
        //        {
        //            var fileStream = System.IO.File.OpenRead(@"C:\\Users\\Defoulse\\Desktop\\image" + i + ".jpg");
        //            using (fileStream)
        //            {
        //                name = fileStream.Name;
        //                blobItem.UploadFromStreamAsync(fileStream).Wait();
        //            }
        //            uploadedFileSentence = uploadedFileSentence + name + " is successfully uploaded to the blob storage!\n";
        //        }
        //        catch (Exception ex)
        //        {
        //            return uploadedFileSentence + "Technical issue: " + ex.ToString() + ". Please upload the file again!";
        //        }
        //    }

        //    return uploadedFileSentence;
        //}

        //public ActionResult ListItemsAsGallery(string Message = null)
        //{
        //    ViewBag.msg = Message;
        //    CloudBlobContainer container = getContainerInformation();
        //    List<string> blobs = new List<string>();
        //    BlobResultSegment result = container.ListBlobsSegmentedAsync(null).Result;

        //    foreach(IListBlobItem item in result.Results)
        //    {
        //        if (item.GetType() == typeof(CloudBlockBlob))
        //        {
        //            CloudBlockBlob blob = (CloudBlockBlob)item;

        //            if (Path.GetExtension(blob.Name.ToString()) == ".jpg")
        //            {
        //                blobs.Add(blob.Name + "#" + blob.Uri.ToString());
        //            }

        //        }
        //        else if (item.GetType() == typeof(CloudPageBlob))
        //        {
        //            CloudPageBlob blob = (CloudPageBlob)item;
        //            blobs.Add(blob.Name + "#" + blob.Uri.ToString());
        //        }
        //        else if (item.GetType() == typeof(CloudBlobDirectory))
        //        {
        //            CloudBlobDirectory blob = (CloudBlobDirectory)item;
        //            blobs.Add(blob.Uri.ToString());
        //        }
        //    }

        //    return View(blobs);
        //}

        public ActionResult DeleteBlob(string area)
        {
            CloudBlobContainer container = getContainerInformation();
            string message = "";

            try
            {
                CloudBlockBlob item = container.GetBlockBlobReference(area);
                message = item.Name + " is deleted from your blob storage!";
                item.DeleteIfExistsAsync();
            }
            catch(Exception ex)
            {
                message = "Unable to delete the file of " + area +
                    "\nTechnical issue: " + ex.ToString() + ". please try again!";
            }

            return RedirectToAction("ListItemsAsGallery", "Blobs", new { Message = "Chosen Blob Deleted!" });
        }

        public ActionResult DownloadBlob(string imagename, string urlimage)
        {
            CloudBlobContainer container = getContainerInformation();
            string message = null;

            try
            {
                CloudBlockBlob item = container.GetBlockBlobReference(imagename);
                var outputItem = System.IO.File.OpenWrite(@"C:\\Users\\Defoulse\\Desktop\\" + imagename);
                item.DownloadToStreamAsync(outputItem).Wait();
                message = imagename + " is successfully downloaded to your desktop! please check it!";
                outputItem.Close();
            }
            catch (Exception ex)
            {
                message = "Unable to download the file of " + imagename +
                    "\nTechnical issue: " + ex.ToString() + ". please try download the file again!";
            }

            return RedirectToAction("ListItemsAsGallery", "Blobs", new { Message = message });
        }

        public IActionResult UploadFile(string contents = null)
        {
            ViewBag.msg = contents;
            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(List<IFormFile> files)
        {
            CloudBlobContainer container = getContainerInformation();
            CloudBlockBlob blobItem = null;

            foreach(var file in files)
            {
                try
                {
                    var stream = file.OpenReadStream();
                    string filename = file.FileName;
                    blobItem = container.GetBlockBlobReference(filename);
                    blobItem.UploadFromStreamAsync(stream).Wait();
                }
                catch(Exception ex)
                {
                    return BadRequest("Technical issue: " + ex.ToString() + ". Please upload the file again!");
                }
            }
            return RedirectToAction("UploadFile", "Blobs", new { contents = "File Uploaded to Blob Storage!" });
        }
    }
}
