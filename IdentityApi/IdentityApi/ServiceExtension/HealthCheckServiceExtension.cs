using Identity.Health;

namespace Identity.ServiceExtension;

public static class HealthCheckServiceExtension
{
    public static void AddHealthCheckService(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<TokenServiceHealthCheck>(TokenServiceHealthCheck.HealthName);

        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.HealthName);
    }
}