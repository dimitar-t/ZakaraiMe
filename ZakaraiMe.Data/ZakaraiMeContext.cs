namespace ZakaraiMe.Data
{
    using Entities.Implementations;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ZakaraiMeContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ZakaraiMeContext(DbContextOptions<ZakaraiMeContext> options) : base(options)
        {
        }

        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasOne(u => u.ProfilePicture);

            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Cars)
                .WithOne(c => c.Owner)
                .HasForeignKey(c => c.OwnerId);

            modelBuilder
                .Entity<Car>()
                .HasOne(c => c.Picture);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
