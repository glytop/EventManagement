using EventsWebApp.Domain.Entities;
using EventsWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Test
{
    public class EventRepositoryTests
    {
        private ApplicationDbContext GetInMemoryDbContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEvents()
        {
            // Arrange
            using var context = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var repository = new EventRepository(context);

            var events = new List<Event>
            {
                new Event
                {
                    Name = "Event 1"
                },
                new Event
                {
                    Name = "Event 2"
                }
            };
            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectEvent()
        {
            // Arrange
            using var context = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var repository = new EventRepository(context);

            var evnt = new Event
            {
                Id = 1,
                Name = "Event 1"
            };
            await context.Events.AddAsync(evnt);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEvent()
        {
            // Arrange
            using var context = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var repository = new EventRepository(context);

            var evnt = new Event
            {
                Name = "New Event"
            };

            // Act
            await repository.AddAsync(evnt);
            await context.SaveChangesAsync();

            // Assert
            Assert.Single(context.Events);
            Assert.Equal("New Event", context.Events.First().Name);
        }
    }
}
