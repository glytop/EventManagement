using EventsWebApp.API.Domain.Entities;
using EventsWebApp.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.API.Infrastructure.Services
{
    public class EventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _unitOfWork.Events.GetAllAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _unitOfWork.Events.GetByIdAsync(id);
        }

        public async Task<List<Event>> GetEventsByCriteriaAsync(string criterion, string value)
        {
            if (string.IsNullOrEmpty(criterion) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Criterion and value must be provided.");
            }

            var query = _unitOfWork.Events.GetEventsByCriterion(criterion, value);
            return await query.ToListAsync();

        }

        public async Task AddEventAsync(Event evnt)
        {
            await _unitOfWork.Events.AddAsync(evnt);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event evnt)
        {
            await _unitOfWork.Events.UpdateAsync(evnt);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            await _unitOfWork.Events.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
