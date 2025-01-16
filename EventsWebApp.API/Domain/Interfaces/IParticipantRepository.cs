using EventsWebApp.API.Domain.DTOs;
using EventsWebApp.API.Domain.Entities;

namespace EventsWebApp.API.Domain.Interfaces
{
    public interface IParticipantRepository
    {
        Task<List<ParticipantDto>> GetParticipantsByEventIdAsync(int eventId);
        Task<ParticipantDto?> GetParticipantByIdAsync(int participantId);
        Task<bool> CancelParticipationAsync(int participantId);

    }
}
