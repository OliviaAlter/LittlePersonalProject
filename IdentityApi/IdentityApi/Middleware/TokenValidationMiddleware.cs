using IdentityCore.ServiceInterface.Token;

namespace Identity.Middleware;

public class TokenValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITokenService tokenService,
        ILogger<TokenValidationMiddleware> logger)
    {
        var authHeader = context.Request.Headers["Authorization"];

        if (authHeader.Count > 0)
        {
            var token = authHeader.ToString().Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                // Extract account information or token id from the token
                var accountId = await tokenService.GetUserIdFromToken(token);

                if (accountId == Guid.Empty)
                {
                    // Handle invalid token: return an error or redirect
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }

                var isValid = await tokenService.IsRefreshTokenValid(token, accountId);

                if (!isValid)
                {
                    // Handle invalid token: return an error or redirect
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }
            }
        }

        await next(context);
    }
}