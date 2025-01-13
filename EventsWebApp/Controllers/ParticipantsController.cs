using EventsWebApp.Domain;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantRepository _participantRepository;

        public ParticipantsController(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterParticipant(Participant participant)
        {
            try
            {
                var registeredParticipant = await _participantRepository.RegisterParticipantAsync(participant);
                return CreatedAtAction(nameof(GetParticipantById), new { id = registeredParticipant.Id }, registeredParticipant);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetParticipantsByEventId(int eventId)
        {
            var participants = await _participantRepository.GetParticipantsByEventIdAsync(eventId);

            if (participants.Count == 0)
            {
                return NotFound(new { Message = "No participants found for this event." });
            }

            return Ok(participants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParticipantById(int id)
        {
            var participant = await _participantRepository.GetParticipantByIdAsync(id);

            if (participant == null)
            {
                return NotFound(new { Message = "Participant not found." });
            }

            return Ok(participant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelParticipation(int id)
        {
            var success = await _participantRepository.CancelParticipationAsync(id);

            if (!success)
            {
                return NotFound(new { Message = "Participant not found." });
            }

            return NoContent();
        }
    }
}
