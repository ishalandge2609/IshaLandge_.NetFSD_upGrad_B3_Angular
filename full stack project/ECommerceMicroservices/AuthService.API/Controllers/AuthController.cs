using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.API.DTOs;
using AuthService.API.Services;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService
                .RegisterAsync(registerDTO);

            return Ok(new
            {
                success = true,
                message = result
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!string.IsNullOrEmpty(result.Error))
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("change-role/{userId}")]
        public async Task<IActionResult> ChangeRole(
            int userId,
            [FromBody] ChangeRoleDTO dto)
        {
            var adminIdClaim = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(adminIdClaim))
            {
                return Unauthorized(new
                {
                    message = "Invalid token"
                });
            }

            int loggedInAdminId = int.Parse(adminIdClaim);

            var result = await _authService.ChangeRoleAsync(
                loggedInAdminId,
                userId,
                dto.Role);

            return Ok(new
            {
                success = true,
                message = result
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();

            return Ok(users.Select(u => new
            {
                u.Id,
                u.Email,
                u.Role,
                u.CreatedAt
            }));
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Email = email,
                Role = role,
                Message = "This is a protected endpoint"
            });
        }
    }
}