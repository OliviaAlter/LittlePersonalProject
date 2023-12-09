using IdentityInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.ServiceExtension;

public static class DatabaseServiceExtension
{
    public static void AddSqlServerService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<IAuthIdentityDbContext, AuthIdentityDbContext>(
            option
                => option.UseSqlServer(configuration["TotallyNotConnectionString:Secret"] ??
                                       throw new InvalidOperationException("Database setting is null")));
    }
}