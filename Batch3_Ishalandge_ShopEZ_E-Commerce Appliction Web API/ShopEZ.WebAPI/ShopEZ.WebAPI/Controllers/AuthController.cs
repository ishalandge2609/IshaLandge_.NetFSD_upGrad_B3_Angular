using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopEZ.WebAPI.DTOs;
using ShopEZ.WebAPI.Services;


namespace ShopEZ.WebAPI.Controllers
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
        //register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
           
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _service.RegisterAsync(dto);
            return Ok(result);
        }
        //login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var token = await _service.LoginAsync(dto);
            return Ok(new { token });
        }
    }
    }
