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

        public DbInitializer(
            IAuthRepository authRepository,
            IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        // Creates database, tables, and seeds admin user
        public async Task InitializeAsync()
        {
            var connectionString =
                _configuration.GetConnectionString("DefaultConnection");

            // First connect to master database
            var masterConnectionString =
                "Server=sqlserver;Database=master;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;Encrypt=False";

            using (var masterConnection =
                   new SqlConnection(masterConnectionString))
            {
                await masterConnection.OpenAsync();

                // Create Auth DB if not exists
                var createDatabaseQuery = @"
                    IF DB_ID('ECommerceAuthDB') IS NULL
                    BEGIN
                        CREATE DATABASE ECommerceAuthDB
                    END";

                await masterConnection.ExecuteAsync(createDatabaseQuery);
            }

            // Now connect to actual Auth DB
            using var connection = new SqlConnection(connectionString);

            await connection.OpenAsync();

            // Create Users table if not exists
            var createTableQuery = @"
                IF NOT EXISTS (
                    SELECT * FROM sysobjects
                    WHERE name='Users' AND xtype='U'
                )
                CREATE TABLE Users (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Email NVARCHAR(100) NOT NULL UNIQUE,
                    PasswordHash NVARCHAR(255) NOT NULL,
                    Role NVARCHAR(50) NOT NULL DEFAULT 'User',
                    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
                )";

            await connection.ExecuteAsync(createTableQuery);

            // Seed Admin User
            await SeedDefaultAdminAsync();
        }

        // Seeds default admin
        private async Task SeedDefaultAdminAsync()
        {
            var adminEmail = "ishalandge234@gmail.com";

            var existingAdmin =
                await _authRepository.GetUserByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var adminUser = new User
                {
                    Email = adminEmail,
                    PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword("Isha@123"),

                    Role = "Admin",

                    CreatedAt = DateTime.UtcNow
                };

                await _authRepository.CreateUserAsync(adminUser);
            }
        }
    }
}