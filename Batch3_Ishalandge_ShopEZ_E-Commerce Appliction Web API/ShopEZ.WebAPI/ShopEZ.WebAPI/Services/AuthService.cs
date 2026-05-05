using Microsoft.IdentityModel.Tokens;
using ShopEZ.WebAPI.DTOs;
using ShopEZ.WebAPI.Helpers;
using ShopEZ.WebAPI.Models;
using ShopEZ.WebAPI.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShopEZ.WebAPI.Exceptions;

namespace ShopEZ.WebAPI.Services
{
    public class AuthService : IAuthService
    {


        private readonly IUserRepository _userRepo;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepo, JwtHelper jwtHelper)
        {
            _userRepo = userRepo;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> RegisterAsync(RegisterDTO dto)
        {
            var existingUser = await _userRepo.GetByEmailAsync(dto.Email);

            if (existingUser != null)
                throw new BadRequestException("User already exists, please login");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),

                //  Admin logic
                Role = dto.Role?.Trim().ToLower() == "admin" ? "Admin" : "User"
            };

            await _userRepo.AddAsync(user);

            return "User registered successfully";
        }

        public async Task<string> LoginAsync(LoginDTO dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!isValid)
                throw new UnauthorizedAccessException("Invalid email or password");

            return _jwtHelper.GenerateToken(user.UserId, user.Email, user.Role);
        }
    }
}
