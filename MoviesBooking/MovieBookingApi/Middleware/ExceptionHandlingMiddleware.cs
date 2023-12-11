using System.Net;
using Common.Exception.HttpException;
using Microsoft.AspNetCore.Mvc;

namespace MovieBookingApi.Middleware;

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
            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case ConflictException:
                statusCode = HttpStatusCode.Conflict;
                message = exception.Message;
                break;
            case ForbiddenAccessException:
                statusCode = HttpStatusCode.Forbidden;
                message = "You do not have permission to perform this action.";
                break;
            case InternalServerErrorException:
                statusCode = HttpStatusCode.InternalServerError;
                message = exception.Message;
                break;
            case NotAcceptableException:
                statusCode = HttpStatusCode.NotAcceptable;
                message = exception.Message;
                break;
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;
            case NotImplementedErrorException:
                statusCode = HttpStatusCode.NotImplemented;
                message = exception.Message;
                break;
            case PreconditionFailedException:
                statusCode = HttpStatusCode.PreconditionFailed;
                message = exception.Message;
                break;
            case RequestTimeoutException:
                statusCode = HttpStatusCode.RequestTimeout;
                message = exception.Message;
                break;
            case ServiceUnavailableException:
                statusCode = HttpStatusCode.ServiceUnavailable;
                message = exception.Message;
                break;
            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "Access is denied for this resource.";
                break;
            case UnsupportedMediaTypeException:
                statusCode = HttpStatusCode.UnsupportedMediaType;
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