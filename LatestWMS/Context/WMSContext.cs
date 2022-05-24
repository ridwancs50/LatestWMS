using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatestWMS.Models;
using Microsoft.AspNetCore.Identity;

namespace LatestWMS.Context
{
    public class WMSContext : IdentityDbContext
    {
        public WMSContext(DbContextOptions<WMSContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        /*    builder.Entity<ApplicationUser>()
                .Property(e => e.FirstName)
                .HasMaxLength(250);

            builder.Entity<ApplicationUser>()
               .Property(e => e.LastName)
               .HasMaxLength(250);

            builder.Entity<ApplicationUser>()
               .Property(e => e.Gender)
               .HasMaxLength(250);*/
        }

        public DbSet<ApplicationUser> AppUser { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
