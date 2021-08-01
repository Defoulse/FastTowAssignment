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
                statuses(context);
                cities(context);
                roles(context);
                adminUser(context);
                userRoles(context);
            }
        }

        public static void statuses(FastTowAssignmentContext context)
        {
            if (context.Statuses.Any())
            {
                return;
            }

            context.Statuses.AddRange(
                new Status { Name = "Order is waiting for the Driver"},
                new Status { Name = "Order is taken by the Driver"},
                new Status { Name = "Order is completed"}
                );
            context.SaveChanges();
        }

        public static void cities(FastTowAssignmentContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            context.Cities.AddRange(
                new City
                {
                    Name = "George Town"
                },
                new City
                {
                    Name = "Kuala Lumpur"
                },
                new City
                {
                    Name = "Ipoh"
                },
                new City
                {
                    Name = "Kuching"
                },
                new City
                {
                    Name = "Johor Bahru"
                },
                new City
                {
                    Name = "Kota Kinabalu"
                },
                new City
                {
                    Name = "Shah Alam"
                },
                new City
                {
                    Name = "Malacca City"
                },
                new City
                {
                    Name = "Miri"
                },
                new City
                {
                    Name = "Petaling Jaya"
                },
                new City
                {
                    Name = "Kuala Terengganu"
                },
                new City
                {
                    Name = "Iskandar Puteri"
                },
                new City
                {
                    Name = "Subang Jaya"
                },
                new City
                {
                    Name = "Kuantan"
                }
            );
            context.SaveChanges();
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
                    LockoutEnabled = true,
                    Name = "Jo",
                    FamilyName = "Lee"
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
