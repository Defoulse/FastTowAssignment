using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FastTowAssignment.Areas.Identity.Data;
using FastTowAssignment.Data;
using FastTowAssignment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FastTowAssignment.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly FastTowAssignmentContext _context;
        private readonly UserManager<FastTowAssignmentUser> _userManager;
        private readonly SignInManager<FastTowAssignmentUser> _signInManager;

        public IndexModel(
            UserManager<FastTowAssignmentUser> userManager,
            SignInManager<FastTowAssignmentUser> signInManager,
            FastTowAssignmentContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [DataType(DataType.Text)]
            [Display(Name = "Profile Picture")]
            public string ProfilePicture { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Family Name")]
            public string FamilyName { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(FastTowAssignmentUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            string url = "";

            Username = userName;

            string loggedInUserId = _userManager.GetUserId(User);
            List<UserMedia> userMedia = (from a in _context.UserMedia where a.UserId == loggedInUserId select a).ToList();

            url = "https://fasttowassignmentstorage.blob.core.windows.net/imagecontainer/Default_Profile_Picture.png";

            if (userMedia != null)
            {
                for (int i = 0; i < userMedia.Count; i++)
                {
                    if (i == 0)
                    {
                        url = userMedia[i].MediaUrl;
                    }
                }
            }

            Input = new InputModel
            {
                Name = user.Name,
                FamilyName = user.FamilyName,
                PhoneNumber = phoneNumber
            };

            ViewData["Picture"] = url;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }
            if (Input.FamilyName != user.FamilyName)
            {
                user.FamilyName = Input.FamilyName;
            }
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
