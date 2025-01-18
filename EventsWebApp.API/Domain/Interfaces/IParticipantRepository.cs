using EventsWebApp.API.Domain.DTOs;
using EventsWebApp.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.API.Domain.Interfaces
{
    public interface IParticipantRepository
    {
        Task<Participant?> GetByIdAsync(int participantId);
        Task<List<Participant>> GetByEventIdAsync(int eventId);
        Task AddAsync(Participant participant);
        void Remove(Participant participant);
    }
}
