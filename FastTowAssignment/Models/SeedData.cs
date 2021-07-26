using FastTowAssignment.Data;
using FastTowAssignment.Models;
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
                roles(context);
                adminUser(context);
                userRoles(context);
            }
        }

        public static void adminUser(FastTowAssignmentContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            context.Users.AddRange(
                new Areas.Identity.Data.FastTowAssignmentUser
                {
                    Id = "d747aa55-cb04-4085-9a4b-d68651d5bd96",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "admin@gmail.com",
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

        public static void roles(FastTowAssignmentContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            context.Roles.AddRange(
                new Microsoft.AspNetCore.Identity.IdentityRole
                {
                    Id = "10afb392-f847-44ee-925c-31808d698ba4",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "d6a91bee-59e8-4ecb-90f0-0805fdb1ea0b"
                },
                new Microsoft.AspNetCore.Identity.IdentityRole
                {
                    Id = "62ff4bb1-e466-43c4-8f48-10ed6676188c",
                    Name = "Client",
                    NormalizedName = "CLIENT",
                    ConcurrencyStamp = "4eeda4b2-5b7c-49bd-ac19-59f8c1bd9f45"
                },
                new Microsoft.AspNetCore.Identity.IdentityRole
                {
                    Id = "9f5f0a51-19d1-4df6-b6d7-147468439887",
                    Name = "Driver",
                    NormalizedName = "DRIVER",
                    ConcurrencyStamp = "3941db8e-1a58-4776-a4d1-c341bac679b3"
                }
            );
            context.SaveChanges();
        }

        public static void userRoles(FastTowAssignmentContext context)
        {
            if (context.UserRoles.Any())
            {
                return;
            }

            context.UserRoles.AddRange(
                new Microsoft.AspNetCore.Identity.IdentityUserRole<string>
                {
                    UserId = "d747aa55-cb04-4085-9a4b-d68651d5bd96",
                    RoleId = "10afb392-f847-44ee-925c-31808d698ba4"
                }
            );
            context.SaveChanges();
        }
    }
}
