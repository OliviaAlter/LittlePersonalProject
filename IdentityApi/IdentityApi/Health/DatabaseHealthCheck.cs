using IdentityInfrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Identity.Health;

public abstract class DatabaseHealthCheck<T>
    (IApplicationDbContext dbContext, ILogger<DatabaseHealthCheck<T>> logger) : IHealthCheck
    where T : IApplicationDbContext
{
    public const string HealthName = "Database_Health_Check";

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Perform a test query or operation on your DbContext
            // For example, you can execute a simple query like this:
            await dbContext.IsDatabaseAvailableAsync(cancellationToken);

            // DbContext is healthy
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            // If an exception is thrown, return an unhealthy result with the error details
            logger.LogError("Database is not available : {Message}", ex.Message);
            return HealthCheckResult.Unhealthy("DbContext connection test failed", ex);
        }
    }
}