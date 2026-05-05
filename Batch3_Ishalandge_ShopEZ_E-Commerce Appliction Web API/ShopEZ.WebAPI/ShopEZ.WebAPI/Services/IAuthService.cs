using ShopEZ.WebAPI.DTOs;

namespace ShopEZ.WebAPI.Services
{
    public interface IAuthService
    {

        Task<string> RegisterAsync(RegisterDTO dto);

        Task<string> LoginAsync(LoginDTO dto);
    }
}
