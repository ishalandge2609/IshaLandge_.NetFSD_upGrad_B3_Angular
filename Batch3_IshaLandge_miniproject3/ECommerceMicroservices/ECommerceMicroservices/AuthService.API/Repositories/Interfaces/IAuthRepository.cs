using AuthService.API.Models;

namespace AuthService.API.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<int> CreateUserAsync(User user);
    }
}