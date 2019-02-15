namespace ZakaraiMe.Data
{
    using Entities.Implementations;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ZakaraiMeContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ZakaraiMeContext(DbContextOptions<ZakaraiMeContext> options) : base(options)
        {
        }

        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasOne(u => u.ProfilePicture);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
