using System.Net;
using System.Text.Json;
using API.Exceptions;

namespace API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    //! Constructor receives the next middleware, logger, and environment info
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    //! This method intercepts the HTTP request pipeline
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //! Pass control to the next middleware
            await _next(context);
        }
        //? Catch custom application exceptions thrown explicitly (e.g., 400, 401, 404)
        //? These are controlled errors created with status code, message, and optional details
        catch (ApiException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;

            //! Create a response object using the exception data
            //! If environment is Development, include detailed error info
            var response = new
            {
                status = ex.StatusCode,
                message = ex.Message,
                details = _env.IsDevelopment() ? ex.Details : null
            };
            //! Configure serialization options to follow camelCase naming
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            //! Serialize the response object and write it to the HTTP response
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
        //? Catch all unhandled exceptions (unexpected server errors)
        //? These typically indicate bugs or issues in the code and return 500 status
        catch (Exception ex)
        {
            //! Log the exception with its message
            _logger.LogError(ex, ex.Message);

            //! Set response type and status code
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //! Create response object with or without details depending on environment
            var response = _env.IsDevelopment()
            ? new
            {
                status = 500,
                message = ex.Message,
                details = ex.StackTrace?.ToString()
            } :
               new
               {
                   status = 500,
                   message = ex.Message,
                   details = (string?)null
               };

            //! Configure JSON serialization options to use camelCase
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            //! Serialize the response object to JSON
            var json = JsonSerializer.Serialize(response, options);

            //! Write the serialized JSON to the response body
            await context.Response.WriteAsync(json);
        }
    }
}

