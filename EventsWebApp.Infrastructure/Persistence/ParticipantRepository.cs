using EventsWebApp.Application.Interfaces;
using EventsWebApp.Domain.DTOs;
using EventsWebApp.Domain.Entities;
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
            return true;
        }


        public async Task<ParticipantDto?> GetParticipantByIdAsync(int participantId)
        {
            return await _context.Participants
                .Where(p => p.Id == participantId)
                .Select(p => new ParticipantDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = $"{p.User.FirstName} {p.User.LastName}",
                    EventId = p.EventId,
                    EventName = p.Event.Name,
                    RegisteredAt = p.RegisteredAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<ParticipantDto>> GetParticipantsByEventIdAsync(int eventId)
        {
            return await _context.Participants
                .Where(p => p.EventId == eventId)
                .Select(p => new ParticipantDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = $"{p.User.FirstName} {p.User.LastName}",
                    EventId = p.EventId,
                    RegisteredAt = p.RegisteredAt
                })
                .ToListAsync();
        }


        public async Task<Participant?> RegisterParticipantAsync(Participant participant)
        {
            var existingParticipant = await _context
                .Participants
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
            return participant;
        }
    }
}
