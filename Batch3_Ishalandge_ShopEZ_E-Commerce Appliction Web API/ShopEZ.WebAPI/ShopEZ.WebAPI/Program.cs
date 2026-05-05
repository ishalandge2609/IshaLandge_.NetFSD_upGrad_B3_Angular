using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopEZ.WebAPI.Data;
using ShopEZ.WebAPI.Helpers;
using ShopEZ.WebAPI.Middleware;
using ShopEZ.WebAPI.Repositories;
using ShopEZ.WebAPI.Services;
using System.Text;

namespace ShopEZ.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            //  Add Services
          

            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            
            // Dependency Injection
    

            // Product
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            // Order
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            // User & Auth
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<JwtHelper>();

           
            //  JWT Authentication
         

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

           
            //  RESEED PRODUCTS TABLE (DEV ONLY)
           

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                //  ONLY FOR TESTING (will delete all products)
                context.Database.ExecuteSqlRaw("DELETE FROM Products");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Products', RESEED, 0)");
            }
            
            //  RESEED PRODUCTS order (DEV ONLY)
           

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                context.Database.ExecuteSqlRaw("DELETE FROM OrderItems");
                context.Database.ExecuteSqlRaw("DELETE FROM Orders");

                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('OrderItems', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Orders', RESEED, 0)");
            }
        
            // Middleware Pipeline
        

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            // Correct order
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}