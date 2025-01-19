using EventsWebApp.Domain.Entities;
using EventsWebApp.Infrastructure.Persistence;
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
                        Date = DateTime.UtcNow.AddDays(1),
                        Category = "Category 1"
                    },
                    new Event
                    {
                        Name = "Event 2",
                        Location = "Location 2",
                        Date = DateTime.UtcNow.AddDays(2),
                        Category = "Category 2"
                    },
                    new Event
                    {
                        Name = "Event 3",
                        Location = "Location 3",
                        Date = DateTime.UtcNow.AddDays(3),
                        Category = "Category 1"
                    }
                );
                await context.SaveChangesAsync();
            }

            return context;
        }

        [Fact]
        public async Task GetAllAsync_Returns_All_Events()
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

        [Fact]
        public async Task GetByIdAsync_Returns_Correct_Event()
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

        [Fact]
        public async Task AddAsync_Adds_Event_Successfully()
        {
            // Arrange
            var context = await GetInMemoryDbContext("AddEventDb");
            var repository = new EventRepository(context);

            var newEvent = new Event
            {
                Name = "New Event",
                Location = "New Location",
                Date = DateTime.UtcNow.AddDays(5),
                Category = "New Category"
            };

            // Act
            await repository.AddAsync(newEvent);
            await context.SaveChangesAsync();

            // Assert
            Assert.Contains(context.Events, e => e.Name == "New Event" && e.Location == "New Location");
        }

        [Fact]
        public async Task UpdateAsync_Updates_Event_Successfully()
        {
            // Arrange
            var context = await GetInMemoryDbContext("UpdateEventDb");
            var repository = new EventRepository(context);

            var eventToUpdate = context.Events.First();
            eventToUpdate.Name = "Updated Name";

            // Act
            await repository.UpdateAsync(eventToUpdate);
            await context.SaveChangesAsync();

            // Assert
            var updatedEvent = await context.Events.FindAsync(eventToUpdate.Id);
            Assert.NotNull(updatedEvent);
            Assert.Equal("Updated Name", updatedEvent.Name);
        }

        [Fact]
        public async Task DeleteAsync_Removes_Event_Successfully()
        {
            // Arrange
            var context = await GetInMemoryDbContext("DeleteEventDb");
            var repository = new EventRepository(context);

            var eventToDelete = context.Events.First();

            // Act
            await repository.DeleteAsync(eventToDelete.Id);
            await context.SaveChangesAsync();

            // Assert
            Assert.DoesNotContain(context.Events, e => e.Id == eventToDelete.Id);
        }

        [Theory]
        [InlineData("name", "Event 1", 1)]
        [InlineData("location", "Location 2", 1)]
        [InlineData("category", "Category 1", 2)]
        public void GetEventsByCriterion_Returns_Filtered_Events(string criterion, string value, int expectedCount)
        {
            // Arrange
            var context = GetInMemoryDbContext("GetEventsByCriterionDb").Result;
            var repository = new EventRepository(context);

            // Act
            var filteredEvents = repository.GetEventsByCriterion(criterion, value).ToList();

            // Assert
            Assert.NotNull(filteredEvents);
            Assert.Equal(expectedCount, filteredEvents.Count);
        }
    }
}
