using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using AuthService.API.Controllers;
using AuthService.API.Services;
using AuthService.API.DTOs;
using AuthService.API.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AuthService.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthenticationService> _mockAuthService;

        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthenticationService>();

            _controller = new AuthController(
                _mockAuthService.Object);
        }

        [Fact]
        public async Task Register_ValidData_ReturnsOkResult()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Email = "test@gmail.com",
                Password = "Test123"
            };

            _mockAuthService
                .Setup(s => s.RegisterAsync(registerDto))
                .ReturnsAsync("User registered successfully");

            // Act
            var result = await _controller
                .Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Register_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState
                .AddModelError("Email", "Email is required");

            var registerDto = new RegisterDTO();

            // Act
            var result = await _controller
                .Register(registerDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Email = "test@gmail.com",
                Password = "Test123"
            };

            var response = new LoginResponseDTO
            {
                Token = "fake-jwt-token",
                Email = "test@gmail.com",
                Role = "User"
            };

            _mockAuthService
                .Setup(s => s.LoginAsync(loginDto))
                .ReturnsAsync(response);

            // Act
            var result = await _controller
                .Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Email = "wrong@gmail.com",
                Password = "Wrong123"
            };

            var response = new LoginResponseDTO
            {
                Error = "Invalid email or password"
            };

            _mockAuthService
                .Setup(s => s.LoginAsync(loginDto))
                .ReturnsAsync(response);

            // Act
            var result = await _controller
                .Login(loginDto);

            // Assert
            var unauthorizedResult =
                Assert.IsType<UnauthorizedObjectResult>(result);

            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task ChangeRole_ValidAdmin_ReturnsOk()
        {
            // Arrange
            var dto = new ChangeRoleDTO
            {
                Role = "Admin"
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            };

            var identity = new ClaimsIdentity(claims);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            _mockAuthService
                .Setup(s => s.ChangeRoleAsync(1, 2, "Admin"))
                .ReturnsAsync("User role updated successfully");

            // Act
            var result = await _controller
                .ChangeRole(2, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task ChangeRole_InvalidToken_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new ChangeRoleDTO
            {
                Role = "Admin"
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller
                .ChangeRole(2, dto);

            // Assert
            var unauthorizedResult =
                Assert.IsType<UnauthorizedObjectResult>(result);

            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkResult()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Email = "admin@gmail.com",
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _mockAuthService
                .Setup(s => s.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller
                .GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void GetProfile_ReturnsUserProfile()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "user@gmail.com"),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            // Act
            var result = _controller.GetProfile();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(200, okResult.StatusCode);
        }
    }
}