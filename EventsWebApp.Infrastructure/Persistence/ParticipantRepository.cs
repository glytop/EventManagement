using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Infrastructure.Persistence
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly ApplicationDbContext _context;

        public ParticipantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Participant?> GetByIdAsync(int participantId)
        {
            return await _context.Participants.FindAsync(participantId);
        }

        public async Task<List<Participant>> GetByEventIdAsync(int eventId)
        {
            return await _context.Participants
                .Where(p => p.EventId == eventId)
                .ToListAsync();
        }

        public async Task AddAsync(Participant participant)
        {
            await _context.Participants.AddAsync(participant);
        }

        public void Remove(Participant participant)
        {
            _context.Participants.Remove(participant);
        }
    }
}
