using EventsWebApp.Domain.Entities;
using EventsWebApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private JwtTokenService _jwtTokenService;

        private static List<User> Users = new()
    {
        new User
        {
            Id = 1,
            Username = "admin",
            Email = "admin@example.com",
            PasswordHash = "admin",
            Roles = new List<string> { "Admin" }
        }
    };

        public AuthController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public IActionResult Login(Domain.Entities.LoginRequest request)
        {
            var user = Users.FirstOrDefault(u => u.Username == request.Username && u.PasswordHash == request.Password);

            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var token = _jwtTokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
