using EventsWebApp.Domain;
using EventsWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Test
{
    public class EventRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public EventRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task AddAsync_ShouldAddEvent()
        {
            // Arrange
            using var context = new ApplicationDbContext(_dbContextOptions);
            var repository = new EventRepository(context);

            var newEvent = new Event
            {
                Name = "Test Event",
                Description = "Description",
                Date = DateTime.UtcNow,
                Location = "Online",
                Category = "Workshop",
                MaxParticipants = 100
            };

            // Act
            await repository.AddAsync(newEvent);

            // Assert
            var addedEvent = await context.Events.FirstOrDefaultAsync(e => e.Name == "Test Event");
            Assert.NotNull(addedEvent);
            Assert.Equal("Test Event", addedEvent.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            using var context = new ApplicationDbContext(_dbContextOptions);
            var repository = new EventRepository(context);

            var existingEvent = new Event
            {
                Name = "Existing Event",
                Description = "Description",
                Date = DateTime.UtcNow,
                Location = "Online",
                Category = "Workshop",
                MaxParticipants = 100
            };
            context.Events.Add(existingEvent);
            await context.SaveChangesAsync();

            // Act
            var retrievedEvent = await repository.GetByIdAsync(existingEvent.Id);

            // Assert
            Assert.NotNull(retrievedEvent);
            Assert.Equal(existingEvent.Id, retrievedEvent.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenEventDoesNotExist()
        {
            // Arrange
            using var context = new ApplicationDbContext(_dbContextOptions);
            var repository = new EventRepository(context);

            // Act
            var retrievedEvent = await repository.GetByIdAsync(999);

            // Assert
            Assert.Null(retrievedEvent);
        }
    }
}
