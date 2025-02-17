
using System.IdentityModel.Tokens.Jwt;
using Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using Xunit;

namespace Services.Tests
{
    public class AuthServiceTests : IDisposable
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            // Set up an in-memory database
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DatabaseContext(options);

            // In-memory configuration for JWT settings
            var inMemorySettings = new Dictionary<string, string>
            {
                { "JwtSettings:SecretKey", "ThisIsASecretKeyForTestingPurposesOnly12345" },
                { "JwtSettings:Issuer", "TestIssuer" },
                { "JwtSettings:Audience", "TestAudience" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Create the AuthService instance using the in-memory context and configuration
            _authService = new AuthService(_context, _configuration);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnTrue_WhenNewUser()
        {
            // Arrange
            string email = "newuser@test.com";
            string password = "Password123";

            // Act
            bool result = await _authService.RegisterUserAsync(email, password);

            // Assert
            Assert.True(result);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
            Assert.NotNull(user);
            Assert.True(BCrypt.Net.BCrypt.Verify(password, user.PasswordHash));
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFalse_WhenUserAlreadyExists()
        {
            // Arrange
            string email = "existinguser@test.com";
            string password = "Password123";
            // Create an existing user
            var existingUser = new User
            {
                EmailAddress = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            // Act
            bool result = await _authService.RegisterUserAsync(email, "AnotherPassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            string email = "nonexistent@test.com";
            string password = "Password123";

            // Act
            var token = await _authService.AuthenticateUserAsync(email, password);

            // Assert
            Assert.Null(token);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            string email = "user@test.com";
            string correctPassword = "CorrectPassword";
            string wrongPassword = "WrongPassword";

            // Create a user
            var user = new User
            {
                EmailAddress = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(correctPassword)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var token = await _authService.AuthenticateUserAsync(email, wrongPassword);

            // Assert
            Assert.Null(token);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnToken_WhenCredentialsAreCorrect()
        {
            // Arrange
            string email = "user@test.com";
            string password = "Password123";

            // Create a user
            var user = new User
            {
                EmailAddress = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var token = await _authService.AuthenticateUserAsync(email, password);

            // Assert
            Assert.NotNull(token);
            // Optionally verify token structure:
            var handler = new JwtSecurityTokenHandler();
            Assert.True(handler.CanReadToken(token));
        }
    }
}
