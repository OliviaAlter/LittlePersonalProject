using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MovieInfrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MovieDbContext>
{
    public MovieDbContext CreateDbContext(string[] args)
    {
        var apiProjectPath = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(),
                "../MovieApi"));

        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<MovieDbContext>();

        optionsBuilder.UseSqlServer(configuration["TotallyNotConnectionString:Secret"] ??
                                    throw new InvalidOperationException("Database setting is null"));

        return new MovieDbContext(optionsBuilder.Options);
    }
}