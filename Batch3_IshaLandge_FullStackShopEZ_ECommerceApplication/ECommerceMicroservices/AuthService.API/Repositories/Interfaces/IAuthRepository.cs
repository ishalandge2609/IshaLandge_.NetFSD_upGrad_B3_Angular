using AuthService.API.Models;
using Microsoft.Data.SqlClient;

namespace AuthService.API.Repositories
{
    // Repository interface for user-related database operations
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserByIdAsync(int id);

        Task<int> CreateUserAsync(User user);

        Task<bool> UpdateUserRoleAsync(int userId, string role);

        Task<IEnumerable<User>> GetAllUsersAsync();

        // Returns SQL database connection
        SqlConnection GetConnection();
    }
}