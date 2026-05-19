using AuthService.API.DTOs;
using AuthService.API.Exceptions;
using AuthService.API.Helpers;
using AuthService.API.Models;
using AuthService.API.Repositories;

namespace AuthService.API.Services
{
    // Handles authentication and user-related business logic
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthRepository _authRepository;

        private readonly JwtHelper _jwtHelper;

        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IAuthRepository authRepository,
            JwtHelper jwtHelper,
            ILogger<AuthenticationService> logger)
        {
            _authRepository = authRepository;

            _jwtHelper = jwtHelper;

            _logger = logger;
        }

        // Registers a new user
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            _logger.LogInformation(
                "User registration started for Email: {Email}",
                registerDTO.Email);

            var existingUser = await _authRepository
                .GetUserByEmailAsync(registerDTO.Email);

            if (existingUser != null)
            {
                _logger.LogWarning(
                    "Registration failed. User already exists: {Email}",
                    registerDTO.Email);

                throw new ValidationException(
                    $"User with email '{registerDTO.Email}' already exists");
            }

            var user = new User
            {
                Email = registerDTO.Email,

                // Password is stored in hashed format
                PasswordHash = BCrypt.Net.BCrypt
                    .HashPassword(registerDTO.Password),

                Role = "User",

                CreatedAt = DateTime.UtcNow
            };

            await _authRepository.CreateUserAsync(user);

            _logger.LogInformation(
                "User registered successfully: {Email}",
                registerDTO.Email);

            return "User registered successfully";
        }

        // Authenticates user and generates JWT token
        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            _logger.LogInformation(
                "Login attempt for Email: {Email}",
                loginDTO.Email);

            var user = await _authRepository
                .GetUserByEmailAsync(loginDTO.Email);

            // Validate user credentials
            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(
                    loginDTO.Password,
                    user.PasswordHash))
            {
                _logger.LogWarning(
                    "Invalid login attempt for Email: {Email}",
                    loginDTO.Email);

                return new LoginResponseDTO
                {
                    Token = null,
                    Email = null,
                    Role = null,
                    Error = "Invalid email or password"
                };
            }

            var token = _jwtHelper.GenerateToken(
                user.Id,
                user.Email,
                user.Role);

            _logger.LogInformation(
                "User logged in successfully: {Email}",
                user.Email);

            return new LoginResponseDTO
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            };
        }

        // Changes user role (Admin/User)
        public async Task<string> ChangeRoleAsync(
            int loggedInAdminId,
            int targetUserId,
            string role)
        {
            _logger.LogInformation(
                "Role change initiated. TargetUserId: {TargetUserId}, NewRole: {Role}",
                targetUserId,
                role);

            var targetUser = await _authRepository
                .GetUserByIdAsync(targetUserId);

            if (targetUser == null)
            {
                throw new ValidationException("User not found");
            }

            // Prevent admin from demoting themselves
            if (loggedInAdminId == targetUserId &&
                role == "User")
            {
                throw new ValidationException(
                    "Admin cannot demote themselves");
            }

            // Validate allowed roles
            if (role != "Admin" && role != "User")
            {
                throw new ValidationException(
                    "Role must be either Admin or User");
            }

            var updated = await _authRepository
                .UpdateUserRoleAsync(targetUserId, role);

            if (!updated)
            {
                throw new Exception("Failed to update role");
            }

            return $"User role updated to {role} successfully";
        }

        // Returns all registered users
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _authRepository.GetAllUsersAsync();
        }
    }
}