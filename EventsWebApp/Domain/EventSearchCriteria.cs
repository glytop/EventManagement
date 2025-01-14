namespace EventsWebApp.API.Domain
{
    public class EventSearchCriteria
    {
        public DateTime? Date { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
    }
}
