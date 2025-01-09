using EventsWebApp.Domain;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evnt = await _repository.GetByIdAsync(id);
            if (evnt == null) return NotFound();
            return Ok(evnt);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event evnt)
        {
            await _repository.AddAsync(evnt);
            return CreatedAtAction(nameof(GetById), new { id = evnt.Id }, evnt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Event evnt)
        {
            if (id != evnt.Id) return BadRequest();
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
