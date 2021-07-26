using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FastTowAssignment.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the FastTowAssignmentUser class
    public class FastTowAssignmentUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public string FamilyName { get; set; }
    }
}
