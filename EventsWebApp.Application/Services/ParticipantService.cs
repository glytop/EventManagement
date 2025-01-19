using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Domain.Interfaces;

namespace EventsWebApp.Application.Services
{
    public class ParticipantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParticipantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CancelParticipationAsync(int participantId)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(participantId);
            if (participant is null)
            {
                return false;
            }

            _unitOfWork.Participants.Remove(participant);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<ParticipantDto?> GetParticipantByIdAsync(int participantId)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(participantId);
            if (participant is null)
            {
                return null;
            }

            return new ParticipantDto
            {
                Id = participant.Id,
                UserId = participant.UserId,
                UserName = $"{participant.User.FirstName} {participant.User.LastName}",
                EventId = participant.EventId,
                EventName = participant.Event.Name,
                RegisteredAt = participant.RegisteredAt
            };
        }

        public async Task<List<ParticipantDto>> GetParticipantsByEventIdAsync(int eventId)
        {
            var participants = await _unitOfWork.Participants.GetByEventIdAsync(eventId);

            return participants.Select(p => new ParticipantDto
            {
                Id = p.Id,
                UserId = p.UserId,
                UserName = $"{p.User.FirstName} {p.User.LastName}",
                EventId = p.EventId,
                RegisteredAt = p.RegisteredAt
            }).ToList();
        }

        public async Task RegisterParticipantAsync(Participant participant)
        {
            var existingParticipant = (await _unitOfWork.Participants.GetByEventIdAsync(participant.EventId))
                .FirstOrDefault(p => p.UserId == participant.UserId);

            if (existingParticipant is not null)
            {
                throw new InvalidOperationException("User is already registered for this event.");
            }

            var evnt = await _unitOfWork.Events.GetByIdAsync(participant.EventId);
            if (evnt is null || evnt.Participants.Count >= evnt.MaxParticipants)
            {
                throw new InvalidOperationException("Cannot register, the event is full or does not exist.");
            }

            await _unitOfWork.Participants.AddAsync(participant);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
