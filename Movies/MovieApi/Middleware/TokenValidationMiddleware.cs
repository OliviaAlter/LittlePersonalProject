using System.Security.Claims;
using MovieApplication.ServiceInterface.JwtToken;

namespace MovieApi.Middleware;

public class TokenValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITokenService tokenService,
        ILogger<TokenValidationMiddleware> logger)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var tokenParts = authHeader.ToString().Split(' ');
            if (tokenParts.Length == 2 && tokenParts[0].Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                var token = tokenParts[1];

                if (!string.IsNullOrWhiteSpace(token))
                    try
                    {
                        var claims = tokenService.GetClaimsFromToken(token);
                        var accountId = claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
                        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                        var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                        if (accountId == null || email == null || username == null)
                        {
                            logger.LogWarning("Missing required claims in the token");
                            context.Response.StatusCode = 401; // Unauthorized
                            return;
                        }

                        var isValid = await tokenService.IsRefreshTokenValid(token);

                        if (!isValid)
                        {
                            logger.LogWarning("Invalid token: token validation failed");
                            context.Response.StatusCode = 401; // Unauthorized
                            return;
                        }

                        // Add account info to HttpContext for downstream access
                        context.Items["AccountId"] = accountId;
                        context.Items["Email"] = email;
                        context.Items["Username"] = username;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error occurred while processing the JWT token.");
                        context.Response.StatusCode = 500; // Internal Server Error
                        return;
                    }
            }
        }

        await next(context);
    }
}