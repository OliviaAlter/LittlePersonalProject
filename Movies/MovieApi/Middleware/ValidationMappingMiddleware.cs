using FluentValidation;
using MovieApi.Response;

namespace MovieApi.Middleware;

public class ValidationMappingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }

        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;

            var validationResponse = new ValidationFailureResponse
            {
                Errors = ex.Errors.Select(x => new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    ErrorMessage = x.ErrorMessage
                })
            };

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(validationResponse);
        }
    }
}