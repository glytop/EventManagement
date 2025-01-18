using EventsWebApp.API.Domain.DTOs;
using EventsWebApp.API.Domain.Entities;
using EventsWebApp.API.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly ParticipantService _participantService;

        public ParticipantsController(ParticipantService participantService)
        {
            _participantService = participantService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterParticipant(RegisterParticipantDto dto)
        {
            var participant = new Participant
            {
                UserId = dto.UserId,
                EventId = dto.EventId,
                RegisteredAt = DateTime.UtcNow
            };

            await _participantService.RegisterParticipantAsync(participant);

            return Ok(new
            {
                Message = "Participant registered successfully",
                Participant = participant
            });
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetParticipantsByEventId(int eventId)
        {
            var participants = await _participantService.GetParticipantsByEventIdAsync(eventId);

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
            var participant = await _participantService.GetParticipantByIdAsync(id);

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
        [Authorize(Policy = "MustBeResourceOwner")]
        public async Task<IActionResult> CancelParticipation(int id)
        {
            var success = await _participantService.CancelParticipationAsync(id);

            if (!success)
            {
                return NotFound(new
                {
                    Message = "Participant not found."
                });
            }

            return Ok(new
            {
                Message = "Participation cancelled successfully."
            });
        }
    }
}
