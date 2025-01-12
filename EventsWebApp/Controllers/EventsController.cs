using EventsWebApp.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Controllers
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


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evnt = await _repository.GetByIdAsync(id);
            if (evnt is null)
            {
                return NotFound();
            }
            return Ok(evnt);
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
