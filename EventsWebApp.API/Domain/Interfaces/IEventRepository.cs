using EventsWebApp.API.Domain.Entities;

namespace EventsWebApp.API.Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task AddAsync(Event evnt);
        Task UpdateAsync(Event evnt);
        Task DeleteAsync(int id);
        IQueryable<Event> GetAllQueryable();
        Task<Event> GetByNameAsync(string name);
        Task<List<Event>> GetByCriteriaAsync(string criterion, string value);
        Task<Event> UpdateEventAsync(Event eventEntity);
    }
}
