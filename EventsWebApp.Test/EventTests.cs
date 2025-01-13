using EventsWebApp.Domain;

namespace EventsWebApp.Test
{
    public class EventTests
    {
        [Fact]
        public void ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var evnt = new Event();

            // Assert
            Assert.NotNull(evnt.Participants);
            Assert.Empty(evnt.Participants);
            Assert.Null(evnt.ImagePath);
        }

        [Fact]
        public void ShouldAddParticipantCorrectly()
        {
            // Arrange
            var evnt = new Event();
            var participant = new Participant
            {
                Id = 1
            };

            // Act
            evnt.Participants.Add(participant);

            // Assert
            Assert.Single(evnt.Participants);
            Assert.Contains(participant, evnt.Participants);
        }
    }
}
