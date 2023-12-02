using IdentityInfrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Identity.Health;

public class DatabaseHealthCheck(IApplicationDbContext applicationDbContext, ILogger<DatabaseHealthCheck> logger)
    : IHealthCheck
{
    public const string HealthName = "Database_Health_Check";

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Example health check logic
            if (await applicationDbContext.CanConnectAsync(cancellationToken)) return HealthCheckResult.Healthy();
            return HealthCheckResult.Unhealthy("Database is not available.");
        }
        catch (Exception ex)
        {
            logger.LogError("Database is not available : {Message}", ex.Message);
            return HealthCheckResult.Unhealthy("Database is not available.", ex);
        }
    }
}