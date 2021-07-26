using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastTowAssignment.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FastTowAssignment.Models;

namespace FastTowAssignment.Data
{
    public class FastTowAssignmentContext : IdentityDbContext<FastTowAssignmentUser>
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public FastTowAssignmentContext(DbContextOptions<FastTowAssignmentContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
