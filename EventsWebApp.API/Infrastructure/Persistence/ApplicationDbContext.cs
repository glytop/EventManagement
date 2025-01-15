using EventsWebApp.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.API.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Participant>()
                .HasOne(p => p.User)
                .WithMany(u => u.Participants)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Participant>()
                .HasOne(p => p.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(p => p.EventId);


            modelBuilder.Entity<User>()
                .Property(u => u.RefreshToken)
                .HasMaxLength(512)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .Property(u => u.RefreshTokenExpiryTime)
                .IsRequired(false);
        }
    }
}
