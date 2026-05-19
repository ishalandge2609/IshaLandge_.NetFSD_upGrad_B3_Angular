using Microsoft.Data.SqlClient;
using System.Data;

namespace AuthService.API.Data
{
    public class DapperContext
    {
        //variable declaration
        private readonly IConfiguration _configuration;
       //constructor
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Creates and returns a SQL database connection
        public IDbConnection CreateConnection()
            => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }
}