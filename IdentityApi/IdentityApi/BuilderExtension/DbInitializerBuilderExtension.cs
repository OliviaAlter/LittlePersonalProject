using IdentityCore.RepositoryInterface.Role;
using IdentityInfrastructure.Data;

namespace Identity.BuilderExtension;

public static class DbInitializerBuilderExtension
{
    public static async Task InitializeRolesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        var repository = services.GetRequiredService<IRoleRepository>();
        try
        {
            await ApplicationDbInitializer.InitializeRoles(repository);
        }

        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<IRoleRepository>>();
            logger.LogError(ex, "Role initialization failed");
        }
    }
}