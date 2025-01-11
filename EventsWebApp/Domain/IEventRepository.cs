namespace EventsWebApp.Domain
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task AddAsync(Event evnt);
        Task UpdateAsync(Event evnt);
        Task DeleteAsync(int id);
        IQueryable<Event> GetAllQueryable();

    }
}
