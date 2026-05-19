using Dapper;
using AuthService.API.Models;
using AuthService.API.Repositories;
using Microsoft.Data.SqlClient;

namespace AuthService.API.Data
{
    public class DbInitializer
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public DbInitializer(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync(); //  Important

            var createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                CREATE TABLE Users (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Email NVARCHAR(100) NOT NULL UNIQUE,
                    PasswordHash NVARCHAR(255) NOT NULL,
                    Role NVARCHAR(50) NOT NULL DEFAULT 'User',
                    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
                )";

            await connection.ExecuteAsync(createTableQuery);

            await SeedDefaultAdminAsync();
        }
        // default admin seeding
        private async Task SeedDefaultAdminAsync()
        {
            var adminEmail = "ishalandge234@gmail.com";

            var existingAdmin = await _authRepository.GetUserByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new User
                {
                    Email = adminEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Isha@123"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                await _authRepository.CreateUserAsync(adminUser);
            }
        }
    }
}