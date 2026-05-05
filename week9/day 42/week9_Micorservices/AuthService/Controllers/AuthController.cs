using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            var result = _service.Register(user);
            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            var token = _service.Login(user.Email, user.Password);

            if (token == null)
                return Unauthorized("Invalid Credentials");

            return Ok(new { Token = token });
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            return Ok(_service.GetAllUsers());
        }
    }
}
