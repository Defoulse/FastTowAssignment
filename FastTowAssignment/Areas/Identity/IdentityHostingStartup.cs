using System;
using FastTowAssignment.Areas.Identity.Data;
using FastTowAssignment.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FastTowAssignment.Areas.Identity.IdentityHostingStartup))]
namespace FastTowAssignment.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<FastTowAssignmentContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("FastTowAssignmentContextConnection")));

                services.AddDefaultIdentity<FastTowAssignmentUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<FastTowAssignmentContext>();
            });
        }
    }
}