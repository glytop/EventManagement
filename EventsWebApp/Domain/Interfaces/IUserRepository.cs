﻿using EventsWebApp.API.Domain.Entities;

namespace EventsWebApp.API.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
