using EventsWebApp.Domain;
using EventsWebApp.Domain.Entities;
using EventsWebApp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(IUserRepository userRepository, JwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user is null || user.PasswordHash != request.Password)
            {
                return Unauthorized(new
                {
                    Message = "Invalid credentials."
                });
            }

            var token = _jwtTokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser is not null)
            {
                return Conflict(new
                {
                    Message = "User with this email already exists."
                });
            }

            user.CreatedAt = DateTime.UtcNow;
            var registeredUser = await _userRepository.RegisterUserAsync(user);

            return CreatedAtAction(nameof(Register), new
            {
                id = registeredUser.Id
            }, registeredUser);
        }
    }
}
