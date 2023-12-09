using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IdentityInfrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthIdentityDbContext>
{
    public AuthIdentityDbContext CreateDbContext(string[] args)
    {
        var apiProjectPath = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(),
                "../IdentityApi"));

        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AuthIdentityDbContext>();

        optionsBuilder.UseSqlServer(configuration["TotallyNotConnectionString:Secret"] ??
                                    throw new InvalidOperationException("Database setting is null"));

        return new AuthIdentityDbContext(optionsBuilder.Options);
    }
}