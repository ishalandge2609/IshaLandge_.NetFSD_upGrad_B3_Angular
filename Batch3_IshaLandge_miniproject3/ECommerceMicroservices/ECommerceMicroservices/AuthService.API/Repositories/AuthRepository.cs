using Dapper;
using AuthService.API.Data;
using AuthService.API.Models;
using Microsoft.Data.SqlClient;

namespace AuthService.API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DapperContext _context;

        public AuthRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }

        public async Task<int> CreateUserAsync(User user)
        {
            var query = @"
                INSERT INTO Users (Email, PasswordHash, Role, CreatedAt) 
                VALUES (@Email, @PasswordHash, @Role, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, user);
        }

        // Helper method for DbInitializer
        public SqlConnection GetConnection()
        {
            return (SqlConnection)_context.CreateConnection();
        }
    }
}