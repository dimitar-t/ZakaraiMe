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
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Message> Messages { get; set; }

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

            modelBuilder
                .Entity<Model>()
                .HasOne(m => m.Make)
                .WithMany(make => make.Models)
                .HasForeignKey(model => model.MakeId);

            modelBuilder
                .Entity<Car>()
                .HasOne(c => c.Model)
                .WithMany(model => model.Cars)
                .HasForeignKey(c => c.ModelId);

            modelBuilder
                .Entity<UserJourney>()
                .HasKey(uj => new { uj.UserId, uj.JourneyId });

            modelBuilder
                .Entity<UserJourney>()
                .HasOne(uj => uj.User)
                .WithMany(u => u.PassengerJourneys)
                .HasForeignKey(uj => uj.UserId);

            modelBuilder
                .Entity<UserJourney>()
                .HasOne(uj => uj.Journey)
                .WithMany(j => j.Passengers)
                .HasForeignKey(uj => uj.JourneyId);

            modelBuilder
                .Entity<User>()
                .HasMany(u => u.DriverJourneys)
                .WithOne(j => j.Driver)
                .HasForeignKey(j => j.DriverId);

            modelBuilder
                .Entity<Journey>()
                .HasOne(j => j.Car)
                .WithMany(c => c.Journeys)
                .HasForeignKey(j => j.CarId);

            modelBuilder
                .Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(r => r.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId);

            modelBuilder
                .Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(s => s.SentMessages)
                .HasForeignKey(m => m.SenderId);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
