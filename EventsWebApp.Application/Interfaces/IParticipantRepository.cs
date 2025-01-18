using EventsWebApp.Domain.DTOs;
using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Application.Interfaces
{
    public interface IParticipantRepository
    {
        Task<Participant?> RegisterParticipantAsync(Participant participant);
        Task<List<ParticipantDto>> GetParticipantsByEventIdAsync(int eventId);
        Task<ParticipantDto?> GetParticipantByIdAsync(int participantId);
        Task<bool> CancelParticipationAsync(int participantId);

    }
}
