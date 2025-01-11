﻿using EventsWebApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Infrastructure.Persistence
{
    public class EventRepository : IEventRepository
    {
        private ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
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

    }
}
