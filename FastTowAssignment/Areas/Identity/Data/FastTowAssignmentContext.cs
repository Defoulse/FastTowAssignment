using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastTowAssignment.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FastTowAssignment.Data
{
    public class FastTowAssignmentContext : IdentityDbContext<FastTowAssignmentUser>
    {
        public FastTowAssignmentContext(DbContextOptions<FastTowAssignmentContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string ADMIN_ID = "d747aa55-cb04-4085-9a4b-d68651d5bd96";
            string ROLE_ID = "10afb392-f847-44ee-925c-31808d698ba4";

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = ADMIN_ID,
                RoleId = ROLE_ID
            });
        }
    }
}
