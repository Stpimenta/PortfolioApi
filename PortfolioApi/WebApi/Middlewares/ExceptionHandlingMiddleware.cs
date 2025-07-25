using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Shared.Exceptions;

namespace PortfolioApi.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task Invoke(HttpContext context)
{
    try
    {
        await _next(context);
    }
    catch (Exception ex)
    {
        context.Response.ContentType = "application/problem+json";
        var response = context.Response;

        ProblemDetails problemDetails;

        switch (ex)
        {
            case BusinessException be:
                _logger.LogWarning("Business exception: {Message}", be.Message);
                problemDetails = new ProblemDetails 
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Title = "Business rule violation",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = be.Message,
                    Instance = context.Request.Path
                };
                response.StatusCode = problemDetails.Status.Value;
                break;

            case NotFoundException nfe:
                _logger.LogInformation("Not found: {Message}", nfe.Message);
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "Resource not found",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = nfe.Message,
                    Instance = context.Request.Path
                };
                response.StatusCode = problemDetails.Status.Value;
                break;
            
            case AuthenticationException ae:
                _logger.LogWarning("Authentication failed: {Message}", ae.Message);
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                    Title = "Unauthorized",
                    Status = (int)HttpStatusCode.Unauthorized,
                    Detail = ae.Message,
                    Instance = context.Request.Path
                };
                response.StatusCode = problemDetails.Status.Value;
                break;

            default:
                _logger.LogError(ex, "Unhandled exception");
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Internal Server Error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = "An unexpected error occurred.",
                    Instance = context.Request.Path
                };
                response.StatusCode = problemDetails.Status.Value;
                break;
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(problemDetails, options);
        
        await response.WriteAsync(json);
    }
}

  
}