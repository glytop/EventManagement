using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Domain
{
    public interface IUserRepository
    {
        Task<User> RegisterUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
