using EventsWebApp.API.Domain.Entities;
using EventsWebApp.API.Domain.Interfaces;
using EventsWebApp.API.Domain.Requests;

namespace EventsWebApp.API.Infrastructure.Services
{
    public class AuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtTokenService _jwtTokenService;

        public AuthService(IUnitOfWork unitOfWork, JwtTokenService jwtTokenService)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<(string token, string refreshToken)> LoginAsync(LoginRequest request)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(request.Email);

            if (user is null || user.PasswordHash != request.Password)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = _jwtTokenService.GenerateToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return (token, refreshToken);
        }

        public async Task<User> RegisterAsync(User user)
        {
            var existingUser = await _unitOfWork.Users.GetUserByEmailAsync(user.Email);
            if (existingUser is not null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            user.CreatedAt = DateTime.UtcNow;
            await _unitOfWork.Users.RegisterUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            var user = await _unitOfWork.Users.GetUserByRefreshTokenAsync(refreshToken);

            if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            return _jwtTokenService.GenerateToken(user);
        }
    }
}
