using EventsWebApp.Domain.Entities;
using EventsWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Test
{
    public class UserRepositoryTests
    {
        private async Task<ApplicationDbContext> GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var context = new ApplicationDbContext(options);

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        FirstName = "Maks",
                        LastName = "Well",
                        Email = "makswell@example.com",
                        PasswordHash = "password",
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        FirstName = "Gly",
                        LastName = "Top",
                        Email = "glytop@example.com",
                        PasswordHash = "anotherpassword",
                        CreatedAt = DateTime.UtcNow
                    }
                );
                await context.SaveChangesAsync();
            }

            return context;
        }

        [Fact]
        public async Task GetUserByEmail_Returns_Correct_User()
        {
            // Arrange
            var context = await GetInMemoryDbContext("GetUserByEmailDb");
            var repository = new UserRepository(context);

            // Act
            var user = await repository.GetUserByEmailAsync("makswell@example.com");

            // Assert
            Assert.NotNull(user);
            Assert.Equal("Maks", user.FirstName);
            Assert.Equal("Well", user.LastName);
        }

        [Fact]
        public async Task RegisterUserAsync_Adds_User_Successfully()
        {
            // Arrange
            var context = await GetInMemoryDbContext("RegisterUserDb");
            var repository = new UserRepository(context);

            var newUser = new User
            {
                FirstName = "New",
                LastName = "User",
                Email = "newuser@example.com",
                PasswordHash = "newpassword",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await repository.RegisterUserAsync(newUser);
            await context.SaveChangesAsync();

            // Assert
            Assert.Contains(context.Users, u => u.Email == "newuser@example.com");
        }
    }
}
