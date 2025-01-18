namespace EventsWebApp.Domain.DTOs
{
    public class ParticipantDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int EventId { get; set; }
        public string EventName { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }
    }
}
