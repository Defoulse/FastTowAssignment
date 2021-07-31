using FastTowAssignment.Areas.Identity.Data;
using FastTowAssignment.Data;
using FastTowAssignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FastTowAssignment.Controllers
{
    public class UserMediaController : Controller
    {
        private readonly FastTowAssignmentContext _context;
        private readonly UserManager<FastTowAssignmentUser> _userManager;
        string ContainerName = "imagecontainer";

        public UserMediaController(UserManager<FastTowAssignmentUser> userManager, FastTowAssignmentContext context)
        {
            _userManager = userManager;
            _context = context;
            utility = new BlobUtility();
        }

        BlobUtility utility;

        [Authorize]
        public IActionResult MediaFileUpload()
        {
            string loggedInUserId = _userManager.GetUserId(User);
            List<UserMedia> userMedia = (from a in _context.UserMedia where a.UserId == loggedInUserId select a).ToList();
            ViewBag.PhotoCount = userMedia.Count;
            return View(userMedia);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadMediaFile(IFormFile file)
        {
            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);
                Stream imageStream = file.OpenReadStream();
                var result = utility.UploadBlobAsync(fileName, ContainerName, (IFormFile)file);
                if (result != null)
                {
                    string loggedInUserId = _userManager.GetUserId(User);
                    UserMedia userMedia = new UserMedia();

                    try
                    {
                        userMedia.UserId = loggedInUserId;
                        userMedia.MediaUrl = result.Result.Uri.ToString();
                        userMedia.MediaFileName = result.Result.Name;
                        userMedia.MediaFileType = result.Result.Name.Split('.').Last();
                    }
                    catch
                    {
                        Console.WriteLine($"Unable to parse '{loggedInUserId}'");
                    }

                    _context.UserMedia.Add(userMedia);
                    _context.SaveChanges();
                    return RedirectToAction("MediaFileUpload");
                }
                else
                {
                    return RedirectToAction("MediaFileUpload");
                }
            }
            else
            {
                return RedirectToAction("MediaFileUpload");
            }
        }

        [Authorize]
        public ActionResult DeleteMediaFile(int id)
        {
            UserMedia userMedia = _context.UserMedia.Find(id);
            _context.UserMedia.Remove(userMedia);
            _context.SaveChanges();
            string BlobNameToDelete = userMedia.MediaUrl.Split('/').Last();
            utility.DeleteBlob(BlobNameToDelete, ContainerName);
            return RedirectToAction("MediaFileUpload");
        }
    }
}
