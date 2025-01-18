using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Infrastructure.Persistence
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context
                .Events
                .Include(e => e.Participants)
                .ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context
                .Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public IQueryable<Event> GetEventsByCriterion(string criterion, string value)
        {
            return criterion.ToLower() switch
            {
                "name" => _context.Events.Where(e => e.Name.Contains(value)),
                "location" => _context.Events.Where(e => e.Location.Contains(value)),
                "category" => _context.Events.Where(e => e.Category.Contains(value)),
                "date" when DateTime.TryParse(value, out var parsedDate) =>
                    _context.Events.Where(e => e.Date.Date == parsedDate.Date),
                _ => throw new ArgumentException("Invalid criterion or value.")
            };
        }

        public async Task AddAsync(Event evnt)
        {
            await _context.Events.AddAsync(evnt);
        }

        public async Task UpdateAsync(Event evnt)
        {
            _context.Events.Update(evnt);
        }

        public async Task DeleteAsync(int id)
        {
            var evnt = await _context.Events.FindAsync(id);
            if (evnt != null)
            {
                _context.Events.Remove(evnt);
            }
        }

    }
}
