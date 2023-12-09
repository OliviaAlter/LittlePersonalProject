using System.Net;
using Microsoft.AspNetCore.Mvc;
using MovieApi.CustomException;

namespace MovieApi.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError; // Default to 500 if not specified
        var message = "An unexpected error has occurred.";

        // Map specific exceptions to HTTP status codes
        switch (exception)
        {
            case ArgumentException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case HttpException.NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "Access is denied for this resource.";
                break;
            case HttpException.ForbiddenAccessException:
                statusCode = HttpStatusCode.Forbidden;
                message = "You do not have permission to perform this action.";
                break;
            case HttpException.ConflictException:
                statusCode = HttpStatusCode.Conflict;
                message = exception.Message;
                break;
        }

        logger.LogError(exception, "An error occurred processing the request");

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = statusCode.ToString(),
            Detail = message
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}