using EventsWebApp.API.Domain.Entities;
using EventsWebApp.API.Domain.Interfaces;
using EventsWebApp.API.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(IUnitOfWork unitOfWork, JwtTokenService jwtTokenService)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(request.Email);

            if (user is null || user.PasswordHash != request.Password)
            {
                return Unauthorized(new
                {
                    Message = "Invalid credentials."
                });
            }

            var token = _jwtTokenService.GenerateToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var existingUser = await _unitOfWork.Users.GetUserByEmailAsync(user.Email);
            if (existingUser is not null)
            {
                return Conflict(new
                {
                    Message = "User with this email already exists."
                });
            }

            user.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.RegisterUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var user = await _unitOfWork.Users.GetUserByRefreshTokenAsync(refreshToken);

            if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized(new
                {
                    Message = "Invalid or expired refresh token."
                });
            }

            var newToken = _jwtTokenService.GenerateToken(user);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
