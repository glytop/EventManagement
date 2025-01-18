using EventsWebApp.Domain.Entities;
using EventsWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Test
{
    public class ParticipantRepositoryTests
    {
        private async Task<ApplicationDbContext> GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var context = new ApplicationDbContext(options);

            if (!context.Participants.Any())
            {
                context.Participants.AddRange(
                    new Participant
                    {
                        UserId = 1,
                        EventId = 1
                    },
                    new Participant
                    {
                        UserId = 2,
                        EventId = 1
                    },
                    new Participant
                    {
                        UserId = 3,
                        EventId = 2
                    }
                );
                await context.SaveChangesAsync();
            }

            return context;
        }

        [Fact]
        public async Task GetByEventId_Returns_Participants_For_Event()
        {
            // Arrange
            var context = await GetInMemoryDbContext("GetParticipantsByEventDb");
            var repository = new ParticipantRepository(context);

            // Act
            var participants = await repository.GetByEventIdAsync(1);

            // Assert
            Assert.NotNull(participants);
            Assert.Equal(2, participants.Count);
            Assert.All(participants, p => Assert.Equal(1, p.EventId));
        }

        [Fact]
        public async Task AddAsync_Adds_Participant_Successfully()
        {
            // Arrange
            var context = await GetInMemoryDbContext("AddParticipantDb");
            var repository = new ParticipantRepository(context);

            var newParticipant = new Participant
            {
                UserId = 4,
                EventId = 3
            };

            // Act
            await repository.AddAsync(newParticipant);
            await context.SaveChangesAsync();

            // Assert
            Assert.Contains(context.Participants, p => p.UserId == 4 && p.EventId == 3);
        }

        [Fact]
        public async Task Remove_Removes_Participant_Successfully()
        {
            // Arrange
            var context = await GetInMemoryDbContext("RemoveParticipantDb");
            var repository = new ParticipantRepository(context);

            var participantToRemove = context.Participants.First();

            // Act
            repository.Remove(participantToRemove);
            await context.SaveChangesAsync();

            // Assert
            Assert.DoesNotContain(context.Participants, p => p.Id == participantToRemove.Id);
        }
    }
}
