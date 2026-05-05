using Ocelot.DependencyInjection;
using Ocelot.Middleware;
namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Load Ocelot configuration
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
           

            // Add Ocelot Services
            builder.Services.AddOcelot();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            // Run Ocelot Middleware
            app.UseOcelot().Wait();
            app.Run();
        }
    }
}
