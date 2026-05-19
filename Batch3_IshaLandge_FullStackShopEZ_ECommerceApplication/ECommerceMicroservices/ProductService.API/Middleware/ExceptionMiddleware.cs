using ProductService.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace ProductService.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;

            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred: {Message}",
                    ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.Clear();

            context.Response.ContentType = "application/json";

            object response;

            switch (exception)
            {
                case ValidationException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.BadRequest;

                    response = new
                    {
                        success = false,
                        message = exception.Message
                    };

                    break;

                case UnauthorizedException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.Unauthorized;

                    response = new
                    {
                        success = false,
                        message = exception.Message
                    };

                    break;

                case NotFoundException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.NotFound;

                    response = new
                    {
                        success = false,
                        message = exception.Message
                    };

                    break;

                default:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;

                    response = new
                    {
                        success = false,
                        message = "An internal server error occurred"
                    };

                    break;
            }

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}