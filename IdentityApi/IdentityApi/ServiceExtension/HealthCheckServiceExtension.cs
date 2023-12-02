using Identity.Health;
using IdentityInfrastructure.Data;

namespace Identity.ServiceExtension;

public static class HealthCheckServiceExtension
{
    public static void AddHealthCheckService(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<TokenServiceHealthCheck>(TokenServiceHealthCheck.HealthName);

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();
    }
}