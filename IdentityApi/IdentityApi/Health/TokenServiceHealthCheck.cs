using IdentityCore.ServiceInterface.Token;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Identity.Health;

public class TokenServiceHealthCheck(ITokenService tokenService, ILogger<TokenServiceHealthCheck> logger) : IHealthCheck
{
    public const string HealthName = "Token_Service_Health_Check";

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Simplified token generation or use a lightweight method
            var token = await tokenService.GenerateHealthCheckTokenAsync();
            return string.IsNullOrEmpty(token)
                ? HealthCheckResult.Unhealthy("Token generation failed.")
                : HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            logger.LogError("Token generation health check failed: {Message}", ex.Message);
            return HealthCheckResult.Unhealthy("Token generation failed.", ex);
        }
    }
}