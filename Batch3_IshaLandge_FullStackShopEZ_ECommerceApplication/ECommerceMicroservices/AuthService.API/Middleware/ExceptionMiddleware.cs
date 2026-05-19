using AuthService.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace AuthService.API.Middleware
{
    // Middleware for handling global exceptions
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
                // Pass request to next middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log unhandled exceptions
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred: {Message}",
                    ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        // Handles exception responses globally
        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            object response;

            switch (exception)
            {
                case ValidationException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.BadRequest;

                    response = new
                    {
                        error = exception.Message,

                        statusCode =
                            (int)HttpStatusCode.BadRequest
                    };

                    break;

                case UnauthorizedException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.Unauthorized;

                    response = new
                    {
                        error = exception.Message,

                        statusCode =
                            (int)HttpStatusCode.Unauthorized
                    };

                    break;

                case NotFoundException:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.NotFound;

                    response = new
                    {
                        error = exception.Message,

                        statusCode =
                            (int)HttpStatusCode.NotFound
                    };

                    break;

                default:
                    context.Response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;

                    response = new
                    {
                        error =
                            "An internal server error occurred",

                        statusCode =
                            (int)HttpStatusCode.InternalServerError
                    };

                    break;
            }

            var jsonResponse =
                JsonSerializer.Serialize(response);

            await context.Response
                .WriteAsync(jsonResponse);
        }
    }
}