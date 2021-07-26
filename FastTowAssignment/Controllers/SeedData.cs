using FastTowAssignment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastTowAssignment.Controllers
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FastTowAssignmentContext(
             serviceProvider.GetRequiredService<DbContextOptions<FastTowAssignmentContext>>()))
            {
                if (context.Users.Any())
                {
                    return;
                }

                context.Users.AddRange(
                    new Areas.Identity.Data.FastTowAssignmentUser
                    {
                        Id = "d747aa55-cb04-4085-9a4b-d68651d5bd96",
                        UserName = "Admin",
                        Email = "admin@gmail.com",
                        NormalizedEmail = "ADMIN@GMAIL.COM",
                        EmailConfirmed = true,
                        PasswordHash = "AQAAAAEAACcQAAAAEJJm39YelHtQm2PcIMc4jp8U5HLqN8j9pSHNZ7givlIKF49l38dzGh6SgPZSAMC2wg==",
                        SecurityStamp = "FDSBIXZPEXL3RHECMWSHKMVRUW3JSIY2",
                        ConcurrencyStamp = "2d700b01-b756-484d-8dbb-79050894ebda",
                        LockoutEnabled = true
                    }
                );
                context.SaveChanges();

            }
        }
    }
}
