using IdentityCore.ServiceInterface.Token;

namespace Identity.Middleware;

public class TokenValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        var authHeader = context.Request.Headers["Authorization"];

        if (authHeader.Count > 0)
        {
            var token = authHeader.ToString().Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                // Extract user information or token id from the token
                var userId = await tokenService.GetUserIdFromToken(token);

                if (userId == Guid.Empty)
                {
                    // Handle invalid token: return an error or redirect
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }

                var isValid = await tokenService.IsRefreshTokenValid(token, userId);

                if (!isValid)
                {
                    // Handle invalid token: return an error or redirect
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }

                // Optionally add user information to HttpContext for downstream access
                //context.Items["User"] = user;
            }
        }

        // Call the next delegate/middleware in the pipeline
        await next(context);
    }
}