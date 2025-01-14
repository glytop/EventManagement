using EventsWebApp.API.Domain;
using EventsWebApp.API.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _repository;

        public EventsController(IEventRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var eventsQuery = _repository.GetAllQueryable();

            var pagedEvents = await eventsQuery
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            var totalItems = await eventsQuery.CountAsync();

            var response = new
            {
                Data = pagedEvents,
                Pagination = new
                {
                    CurrentPage = paginationParams.PageNumber,
                    paginationParams.PageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)paginationParams.PageSize)
                }
            };

            return Ok(response);
        }

        [HttpPost("{id}/add-image-url")]
        public async Task<IActionResult> AddImageUrl(int id, [FromBody] Event request)
        {
            var evnt = await _repository.GetByIdAsync(id);
            if (evnt is null)
            {
                return NotFound(new
                {
                    Message = $"Event with ID {id} not found."
                });
            }

            evnt.ImagePath = request.ImagePath;
            await _repository.UpdateAsync(evnt);

            return Ok(new
            {
                Message = "Image URL added successfully."
            });
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evnt = await _repository.GetByIdAsync(id);
            if (evnt is null)
            {
                return NotFound(new
                {
                    Message = "Event not found."
                });
            }

            var result = new
            {
                evnt.Id,
                evnt.Name,
                evnt.Description,
                evnt.Date,
                evnt.Location,
                evnt.Category,
                evnt.MaxParticipants,
                evnt.ImagePath,
                Participants = evnt.Participants.Select(p => new
                {
                    p.Id,
                    p.User.FirstName,
                    p.User.LastName,
                    p.User.Email
                })
                .ToList()
            };

            return Ok(result);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var evnt = await _repository.GetByNameAsync(name);
            if (evnt is null)
            {
                return NotFound();
            }
            return Ok(evnt);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetByCriteria([FromQuery] string criterion, [FromQuery] string value)
        {
            var events = await _repository.GetByCriteriaAsync(criterion, value);
            if (events is null)
            {
                return NotFound();
            }
            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event evnt)
        {
            await _repository.AddAsync(evnt);
            return Ok(evnt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Event evnt)
        {
            if (id != evnt.Id)
            {
                return BadRequest();
            }
            await _repository.UpdateAsync(evnt);
            return Ok(evnt);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}
