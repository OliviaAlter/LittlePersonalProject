using MovieInfrastructure.Data;

namespace MovieApi.ServiceExtension;

public static class HealthCheckServiceExtension
{
    public static void AddHealthCheckService(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<MovieDbContext>();
    }
}