using AuthService.API.DTOs;

namespace AuthService.API.Services
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO, string role = "User");
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO);
    }
}