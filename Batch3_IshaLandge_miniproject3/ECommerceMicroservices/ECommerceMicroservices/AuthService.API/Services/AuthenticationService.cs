using AuthService.API.DTOs;
using AuthService.API.Exceptions;
using AuthService.API.Helpers;
using AuthService.API.Models;
using AuthService.API.Repositories;

namespace AuthService.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthenticationService(IAuthRepository authRepository, JwtHelper jwtHelper)
        {
            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> RegisterAsync(RegisterDTO registerDTO, string role = "User")
        {
            var existingUser = await _authRepository.GetUserByEmailAsync(registerDTO.Email);

            if (existingUser != null)
                throw new ValidationException($"User with email '{registerDTO.Email}' already exists");

            var user = new User
            {
                Email = registerDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            await _authRepository.CreateUserAsync(user);

            return $"{role} registered successfully";
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _authRepository.GetUserByEmailAsync(loginDTO.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
            {
                return new LoginResponseDTO
                {
                    Token = null,
                    Email = null,
                    Role = null,
                    Error = "Invalid email or password"
                };
            }

            var token = _jwtHelper.GenerateToken(user.Id, user.Email, user.Role);

            return new LoginResponseDTO
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}