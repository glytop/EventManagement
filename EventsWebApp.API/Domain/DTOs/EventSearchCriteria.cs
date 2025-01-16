namespace EventsWebApp.API.Domain.DTOs
{
    public class EventSearchCriteria
    {
        public DateTime? Date { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
    }
}
