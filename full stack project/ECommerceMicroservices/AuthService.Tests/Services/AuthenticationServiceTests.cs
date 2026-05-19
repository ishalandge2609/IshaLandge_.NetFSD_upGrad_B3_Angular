using AuthService.API.DTOs;
using AuthService.API.Exceptions;
using AuthService.API.Helpers;
using AuthService.API.Models;
using AuthService.API.Repositories;
using AuthService.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AuthService.Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IAuthRepository> _mockRepo;

        private readonly Mock<ILogger<AuthenticationService>> _mockLogger;

        private readonly JwtHelper _jwtHelper;

        private readonly AuthenticationService _service;

        public AuthenticationServiceTests()
        {
            _mockRepo = new Mock<IAuthRepository>();

            _mockLogger =
                new Mock<ILogger<AuthenticationService>>();

            var configuration =
                new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        {
                            "Jwt:Key",
                            "ThisIsMySuperSecretJWTKeyForTesting12345"
                        },

                        {
                            "Jwt:Issuer",
                            "TestIssuer"
                        },

                        {
                            "Jwt:Audience",
                            "TestAudience"
                        }
                    }!)
                .Build();

            _jwtHelper = new JwtHelper(configuration);

            _service = new AuthenticationService(
                _mockRepo.Object,
                _jwtHelper,
                _mockLogger.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldRegisterUserSuccessfully()
        {
            // Arrange

            var dto = new RegisterDTO
            {
                Email = "test@gmail.com",

                Password = "123456"
            };

            _mockRepo
                .Setup(r =>
                    r.GetUserByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            // Act

            var result =
                await _service.RegisterAsync(dto);

            // Assert

            Assert.Equal(
                "User registered successfully",
                result);

            _mockRepo.Verify(
                r => r.CreateUserAsync(
                    It.IsAny<User>()),
                Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenUserAlreadyExists()
        {
            // Arrange

            var dto = new RegisterDTO
            {
                Email = "existing@gmail.com",

                Password = "123456"
            };

            _mockRepo
                .Setup(r =>
                    r.GetUserByEmailAsync(dto.Email))
                .ReturnsAsync(new User());

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(
                () => _service.RegisterAsync(dto));
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange

            var password = "123456";

            var hashedPassword =
                BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Id = 1,

                Email = "test@gmail.com",

                PasswordHash = hashedPassword,

                Role = "User"
            };

            var dto = new LoginDTO
            {
                Email = "test@gmail.com",

                Password = password
            };

            _mockRepo
                .Setup(r =>
                    r.GetUserByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            // Act

            var result =
                await _service.LoginAsync(dto);

            // Assert

            Assert.NotNull(result.Token);

            Assert.Equal(
                user.Email,
                result.Email);

            Assert.Equal(
                user.Role,
                result.Role);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnError_WhenCredentialsAreInvalid()
        {
            // Arrange

            var dto = new LoginDTO
            {
                Email = "wrong@gmail.com",

                Password = "wrongpass"
            };

            _mockRepo
                .Setup(r =>
                    r.GetUserByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            // Act

            var result =
                await _service.LoginAsync(dto);

            // Assert

            Assert.Null(result.Token);

            Assert.Equal(
                "Invalid email or password",
                result.Error);
        }

        [Fact]
        public async Task ChangeRoleAsync_ShouldUpdateRoleSuccessfully()
        {
            // Arrange

            var user = new User
            {
                Id = 2,

                Email = "user@gmail.com",

                Role = "User"
            };

            _mockRepo
                .Setup(r =>
                    r.GetUserByIdAsync(2))
                .ReturnsAsync(user);

            _mockRepo
                .Setup(r =>
                    r.UpdateUserRoleAsync(
                        2,
                        "Admin"))
                .ReturnsAsync(true);

            // Act

            var result =
                await _service.ChangeRoleAsync(
                    1,
                    2,
                    "Admin");

            // Assert

            Assert.Equal(
                "User role updated to Admin successfully",
                result);
        }

        [Fact]
        public async Task ChangeRoleAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange

            _mockRepo
                .Setup(r =>
                    r.GetUserByIdAsync(99))
                .ReturnsAsync((User?)null);

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(
                () => _service.ChangeRoleAsync(
                    1,
                    99,
                    "Admin"));
        }

        [Fact]
        public async Task ChangeRoleAsync_ShouldThrowException_WhenRoleIsInvalid()
        {
            // Arrange

            var user = new User
            {
                Id = 2,
                Email = "user@gmail.com",
                Role = "User"
            };

            _mockRepo
                .Setup(r => r.GetUserByIdAsync(2))
                .ReturnsAsync(user);

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(
                () => _service.ChangeRoleAsync(
                    1,
                    2,
                    "Manager"));
        }

        [Fact]
        public async Task ChangeRoleAsync_ShouldThrowException_WhenAdminDemotesSelf()
        {
            // Arrange

            var admin = new User
            {
                Id = 1,
                Email = "admin@gmail.com",
                Role = "Admin"
            };

            _mockRepo
                .Setup(r => r.GetUserByIdAsync(1))
                .ReturnsAsync(admin);

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(
                () => _service.ChangeRoleAsync(
                    1,
                    1,
                    "User"));
        }

        [Fact]
        public async Task ChangeRoleAsync_ShouldThrowException_WhenUpdateFails()
        {
            // Arrange

            var user = new User
            {
                Id = 2,
                Email = "user@gmail.com",
                Role = "User"
            };

            _mockRepo
                .Setup(r => r.GetUserByIdAsync(2))
                .ReturnsAsync(user);

            _mockRepo
                .Setup(r => r.UpdateUserRoleAsync(2, "Admin"))
                .ReturnsAsync(false);

            // Act & Assert

            await Assert.ThrowsAsync<Exception>(
                () => _service.ChangeRoleAsync(
                    1,
                    2,
                    "Admin"));
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnUsers()
        {
            // Arrange

            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Email = "admin@gmail.com",
                    Role = "Admin"
                },

                new User
                {
                    Id = 2,
                    Email = "user@gmail.com",
                    Role = "User"
                }
            };

            _mockRepo
                .Setup(r => r.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act

            var result =
                await _service.GetAllUsersAsync();

            // Assert

            Assert.Equal(2, result.Count());
        }
    }
}