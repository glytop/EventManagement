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
        public async Task<IActionResult> AddImageUrl(int id, [FromBody] string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return BadRequest(new
                {
                    Message = "Image URL is required."
                });
            }

            var evnt = await _repository.GetByIdAsync(id);
            if (evnt is null)
            {
                return NotFound(new
                {
                    Message = "Event not found."
                });
            }

            evnt.ImagePath = imageUrl;
            await _repository.UpdateEventAsync(evnt);

            return Ok(new
            {
                Message = "Image URL updated successfully.",
                evnt.ImagePath
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
        public async Task<IActionResult> GetByCriteria([FromQuery] EventSearchCriteria criteria)
        {
            var events = await _repository.GetByCriteriaAsync(criteria);
            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event evnt)
        {
            await _repository.AddAsync(evnt);
            return CreatedAtAction(nameof(GetById), new
            {
                kid = evnt.Id
            }, evnt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Event evnt)
        {
            if (id != evnt.Id)
            {
                return BadRequest();
            }
            await _repository.UpdateAsync(evnt);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
