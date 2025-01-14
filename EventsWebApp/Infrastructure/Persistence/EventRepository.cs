using EventsWebApp.API.Domain;
using EventsWebApp.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.API.Infrastructure.Persistence
{
    public class EventRepository : IEventRepository
    {
        private ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Event> UpdateEventAsync(Event eventEntity)
        {
            _context.Events.Update(eventEntity);
            await _context.SaveChangesAsync();
            return eventEntity;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context
                .Events
                .Include(x => x.Participants)
                .ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context
                .Events
                .Include(x => x.Participants)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Event evnt)
        {
            await _context
                .Events
                .AddAsync(evnt);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event evnt)
        {
            _context
                .Events
                .Update(evnt);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var evnt = await _context
                .Events
                .FindAsync(id);
            if (evnt != null)
            {
                _context
                    .Events
                    .Remove(evnt);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Event> GetAllQueryable()
        {
            return _context.Events.AsQueryable();
        }

        public async Task<Event> GetByNameAsync(string name)
        {
            return await _context
                .Events
                .FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<List<Event>> GetByCriteriaAsync(EventSearchCriteria criteria)
        {
            var query = _context.Events.AsQueryable();

            if (criteria.Date.HasValue)
            {
                query = query.Where(e => e.Date.Date == criteria.Date.Value.Date);
            }

            if (!string.IsNullOrEmpty(criteria.Location))
            {
                query = query.Where(e => e.Location.Contains(criteria.Location));
            }

            if (!string.IsNullOrEmpty(criteria.Category))
            {
                query = query.Where(e => e.Category.Contains(criteria.Category));
            }

            return await query.ToListAsync();
        }

    }
}
