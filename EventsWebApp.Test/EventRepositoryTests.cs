using EventsWebApp.API.Domain.Entities;
using EventsWebApp.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Test
{
    public class EventRepositoryTests
    {
        private async Task<ApplicationDbContext> GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var context = new ApplicationDbContext(options);

            if (!context.Events.Any())
            {
                context.Events.AddRange(
                    new Event
                    {
                        Name = "Event 1",
                        Location = "Location 1",
                        Date = DateTime.UtcNow.AddDays(1)
                    },
                    new Event
                    {
                        Name = "Event 2",
                        Location = "Location 2",
                        Date = DateTime.UtcNow.AddDays(2)
                    },
                    new Event
                    {
                        Name = "Event 3",
                        Location = "Location 3",
                        Date = DateTime.UtcNow.AddDays(3)
                    }
                );
                await context.SaveChangesAsync();
            }

            return context;
        }

        [Fact]
        public async Task GetAll_Returns_All_Events()
        {
            // Arrange
            var context = await GetInMemoryDbContext("GetAllEventsDb");
            var repository = new EventRepository(context);

            // Act
            var events = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(events);
            Assert.Equal(3, events.Count());
            Assert.Contains(events, e => e.Name == "Event 1");
            Assert.Contains(events, e => e.Name == "Event 2");
            Assert.Contains(events, e => e.Name == "Event 3");
        }

        [Theory]
        [InlineData("Event 4", "Location 4")]
        [InlineData("Event 5", "Location 5")]
        public async Task AddEventAsync_Adds_Event_Successfully(string eventName, string location)
        {
            // Arrange
            var context = await GetInMemoryDbContext("AddEventDb");
            var repository = new EventRepository(context);
            var newEvent = new Event
            {
                Name = eventName,
                Location = location,
                Date = DateTime.UtcNow.AddDays(5)
            };

            // Act
            await repository.AddAsync(newEvent);

            // Assert
            var allEvents = await repository.GetAllAsync();
            Assert.Contains(allEvents, e => e.Name == eventName && e.Location == location);
        }

        [Fact]
        public async Task GetEventById_Returns_Correct_Event()
        {
            // Arrange
            var context = await GetInMemoryDbContext("GetEventByIdDb");
            var repository = new EventRepository(context);

            var expectedEvent = context.Events.First();

            // Act
            var actualEvent = await repository.GetByIdAsync(expectedEvent.Id);

            // Assert
            Assert.NotNull(actualEvent);
            Assert.Equal(expectedEvent.Name, actualEvent.Name);
            Assert.Equal(expectedEvent.Location, actualEvent.Location);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(999, false)]
        public async Task GetEventById_Returns_Null_For_Invalid_Id(int eventId, bool exists)
        {
            // Arrange
            var context = await GetInMemoryDbContext("GetEventByIdInvalidDb");
            var repository = new EventRepository(context);

            // Act
            var actualEvent = await repository.GetByIdAsync(eventId);

            // Assert
            if (exists)
            {
                Assert.NotNull(actualEvent);
            }
            else
            {
                Assert.Null(actualEvent);
            }
        }
    }
}
