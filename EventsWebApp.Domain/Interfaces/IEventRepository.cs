using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        IQueryable<Event> GetEventsByCriterion(string criterion, string value);
        Task AddAsync(Event evnt);
        Task UpdateAsync(Event evnt);
        Task DeleteAsync(int id);
    }
}
