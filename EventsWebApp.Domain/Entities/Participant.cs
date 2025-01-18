namespace EventsWebApp.Domain.Entities
{
    public class Participant
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
