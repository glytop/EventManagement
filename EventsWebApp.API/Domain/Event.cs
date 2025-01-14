namespace EventsWebApp.API.Domain
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int MaxParticipants { get; set; }
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
        public string? ImagePath { get; set; }
    }
}
