using Microsoft.EntityFrameworkCore;
using MovieInfrastructure.Data;

namespace MovieApi.ServiceExtension;

public static class DatabaseServiceExtension
{
    public static void AddSqlServerService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IMovieDbContext, MovieDbContext>(
            option
                => option.UseSqlServer(configuration["TotallyNotConnectionString:Secret"] ??
                                       throw new InvalidOperationException("Database setting is null")));
    }
}