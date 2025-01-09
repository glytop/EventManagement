using EventsWebApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;
    }
}
