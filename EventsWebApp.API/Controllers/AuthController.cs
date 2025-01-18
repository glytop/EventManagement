using EventsWebApp.Application.Requests;
using EventsWebApp.Application.Services;
using EventsWebApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            var (token, refreshToken) = await _authService.LoginAsync(request);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken
            });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var registeredUser = await _authService.RegisterAsync(user);

            return CreatedAtAction(nameof(Register), new
            {
                id = registeredUser.Id
            }, registeredUser);

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {

            var newToken = await _authService.RefreshTokenAsync(refreshToken);

            return Ok(new
            {
                Token = newToken
            });

        }
    }
}
