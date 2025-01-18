using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Domain.Interfaces
{
    public interface IParticipantRepository
    {
        Task<Participant?> GetByIdAsync(int participantId);
        Task<List<Participant>> GetByEventIdAsync(int eventId);
        Task AddAsync(Participant participant);
        void Remove(Participant participant);
    }
}
