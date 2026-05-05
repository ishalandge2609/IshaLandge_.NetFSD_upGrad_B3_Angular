namespace ShopEZ.WebAPI.Middleware;

using System.Net;
using System.Text.Json;
using ShopEZ.WebAPI.Helpers;
using ShopEZ.WebAPI.Exceptions;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleException(context, ex.Message, HttpStatusCode.Unauthorized);
        }
        catch (BadRequestException ex)
        {
            await HandleException(context, ex.Message, HttpStatusCode.BadRequest);
        }

        
        catch (NotFoundException ex)
        {
            await HandleException(context, ex.Message, HttpStatusCode.NotFound);
        }

        catch (Exception ex)
        {
            await HandleException(context, "Something went wrong", HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleException(HttpContext context, string message, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ApiResponse<string>
        {
            Success = false,
            Message = message,
            Data = null,
            StatusCode = (int)statusCode
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}