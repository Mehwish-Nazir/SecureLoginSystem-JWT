using BackeEndAuthentication.Middleware;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using BackeEndAuthentication.CustomExceptions;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using BackeEndAuthentication.Exceptions;


/*
 * register middleware in program.cs file like this 
app.UseMiddleWare< ErrorHandlingMiddleware >
*/
namespace BackeEndAuthentication.Middleware
{

    /*
     JsonSerializer.Serialize
Converts an error object into JSON format for structured responses, like below
    {
  "success": false,
  "message": "Something went wrong",
  "status": 500,
  "timestamp": "2025-05-22T15:30:00Z"
}
     */

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next,ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation(" Entered ErrorHandlingMiddleware");

            try
            {
                await _next(context);
            }
            catch (ConflictException ex)
            {
                //  Custom handling for ConflictException (409)
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = ex.Message,
                    status = context.Response.StatusCode,
                    path = context.Request.Path,
                    timestamp = DateTime.UtcNow
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            catch (InvalidEmailException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    status = 401,
                    message = ex.Message,
                    error = "EmailError"
                });
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogWarning(" Caught InvalidPasswordException: {Message}", ex.Message);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    status = 401,
                    message = ex.Message,
                    error = "PasswordError"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var response = new
                {
                    status = 401,
                    message = ex.Message,
                    error = "Unauthorized"
                };

                await context.Response.WriteAsJsonAsync(response);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occur: {Path}", context.Request.Path);
               context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    //NotFoundException => StatusCodes.Status404NotFound,
                    BadImageFormatException=>StatusCodes.Status415UnsupportedMediaType,
                    BadHttpRequestException => StatusCodes.Status400BadRequest,
                    ConflictException=>StatusCodes.Status409Conflict,
                    _=>StatusCodes.Status500InternalServerError
                    /*'_=>  it is necessary  to add at end  '*/
                };
                var errorResponse = new
                {
                    success = false,
                    message = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred.",
                    status = context.Response.StatusCode,
                    path = context.Request.Path,

                    timestamp = DateTime.UtcNow
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }
}
