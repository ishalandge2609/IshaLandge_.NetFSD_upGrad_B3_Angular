using AuthService.API.DTOs;
using AuthService.API.Models;

namespace AuthService.API.Services
{
    // Service interface for authentication and user management
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);

        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO);

        Task<string> ChangeRoleAsync(
            int loggedInAdminId,
            int targetUserId,
            string role
        );

        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}