using Dapper;
using AuthService.API.Data;
using AuthService.API.Models;
using Microsoft.Data.SqlClient;

namespace AuthService.API.Repositories
{
    // Repository implementation for user database operations
    public class AuthRepository : IAuthRepository
    {
        private readonly DapperContext _context;

        public AuthRepository(DapperContext context)
        {
            _context = context;
        }

        // Fetch user by email
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";

            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<User>(
                query,
                new { Email = email }
            );
        }

        // Fetch user by ID
        public async Task<User?> GetUserByIdAsync(int id)
        {
            var query = "SELECT * FROM Users WHERE Id = @Id";

            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<User>(
                query,
                new { Id = id }
            );
        }

        // Insert new user into database
        public async Task<int> CreateUserAsync(User user)
        {
            var query = @"
                INSERT INTO Users (Email, PasswordHash, Role, CreatedAt)
                VALUES (@Email, @PasswordHash, @Role, @CreatedAt);

                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            using var connection = _context.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                query,
                user
            );
        }

        // Update user role
        public async Task<bool> UpdateUserRoleAsync(
            int userId,
            string role)
        {
            var query = @"
                UPDATE Users
                SET Role = @Role
                WHERE Id = @UserId
            ";

            using var connection = _context.CreateConnection();

            var rowsAffected = await connection.ExecuteAsync(
                query,
                new
                {
                    UserId = userId,
                    Role = role
                });

            return rowsAffected > 0;
        }

        // Fetch all users
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var query = @"
                SELECT
                    Id,
                    Email,
                    Role,
                    CreatedAt
                FROM Users
                ORDER BY Id DESC
            ";

            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<User>(query);
        }

        // Returns database connection for DbInitializer
        public SqlConnection GetConnection()
        {
            return (SqlConnection)_context.CreateConnection();
        }
    }
}