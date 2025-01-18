using EventsWebApp.Application.Services;
using EventsWebApp.Domain.Common;
using EventsWebApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var events = await _eventService.GetAllEventsAsync();

            var pagedEvents = events
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize);

            var response = new
            {
                Data = pagedEvents,
                Pagination = new
                {
                    paginationParams.PageNumber,
                    paginationParams.PageSize,
                    TotalItems = events.Count(),
                    TotalPages = (int)Math.Ceiling(events.Count() / (double)paginationParams.PageSize)
                }
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evnt = await _eventService.GetEventByIdAsync(id);

            if (evnt is null)
            {
                return NotFound(new
                {
                    Message = "Event not found."
                });
            }

            return Ok(evnt);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string criterion, [FromQuery] string value)
        {
            var events = await _eventService.GetEventsByCriteriaAsync(criterion, value);
            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Event evnt)
        {
            await _eventService.AddEventAsync(evnt);
            return CreatedAtAction(nameof(GetById), new { id = evnt.Id }, evnt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Event evnt)
        {
            if (id != evnt.Id)
            {
                return BadRequest(new
                {
                    Message = "Event ID mismatch."
                });
            }

            await _eventService.UpdateEventAsync(evnt);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventService.DeleteEventAsync(id);
            return Ok();
        }
    }
}
