using EventsWebApp.API.Domain;
using EventsWebApp.API.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParticipantsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterParticipant(RegisterParticipantDto dto)
        {
            var participant = new Participant
            {
                UserId = dto.UserId,
                EventId = dto.EventId
            };

            var registeredParticipant = await _unitOfWork.Participants.RegisterParticipantAsync(participant);
            await _unitOfWork.SaveChangesAsync();

            return Ok(registeredParticipant);
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetParticipantsByEventId(int eventId)
        {
            var participants = await _unitOfWork.Participants.GetParticipantsByEventIdAsync(eventId);

            if (participants.Count == 0)
            {
                return NotFound(new
                {
                    Message = "No participants found for this event."
                });
            }

            return Ok(participants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParticipantById(int id)
        {
            var participant = await _unitOfWork.Participants.GetParticipantByIdAsync(id);

            if (participant is null)
            {
                return NotFound(new
                {
                    Message = "Participant not found."
                });
            }

            return Ok(participant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelParticipation(int id)
        {
            var success = await _unitOfWork.Participants.CancelParticipationAsync(id);

            if (!success)
            {
                return NotFound(new
                {
                    Message = "Participant not found."
                });
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
