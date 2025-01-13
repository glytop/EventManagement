using EventsWebApp.Domain;
using EventsWebApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Infrastructure.Persistence
{
    public class ParticipantRepository : IParticipantRepository
    {
        private ApplicationDbContext _context;

        public ParticipantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CancelParticipationAsync(int participantId)
        {
            var participant = await _context.Participants.FindAsync(participantId);

            if (participant is null)
            {
                return false;
            }

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Participant?> GetParticipantByIdAsync(int participantId)
        {
            return await _context.Participants
                .Include(p => p.User)
                .Include(p => p.Event)
                .FirstOrDefaultAsync(p => p.Id == participantId);
        }

        public async Task<List<Participant>> GetParticipantsByEventIdAsync(int eventId)
        {
            return await _context.Participants
                .Where(p => p.EventId == eventId)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Participant?> RegisterParticipantAsync(Participant participant)
        {
            var existingParticipant = await _context.Participants
                .FirstOrDefaultAsync(p => p.UserId == participant.UserId && p.EventId == participant.EventId);

            if (existingParticipant is not null)
            {
                throw new InvalidOperationException("User is already registered for this event.");
            }

            var evnt = await _context.Events.FirstOrDefaultAsync(e => e.Id == participant.EventId);
            if (evnt is null || evnt.Participants.Count >= evnt.MaxParticipants)
            {
                throw new InvalidOperationException("Cannot register, the event is full or does not exist.");
            }

            await _context.Participants.AddAsync(participant);
            await _context.SaveChangesAsync();
            return participant;
        }
    }
}
