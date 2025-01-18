namespace EventsWebApp.API.Domain.Common
{
    public class EventSearchCriteria
    {
        public DateTime? Date { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
    }
}
