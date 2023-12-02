using IdentityCore.RepositoryInterface.Role;

namespace IdentityInfrastructure.Data;

public abstract class ApplicationDbInitializer
{
    public static async Task InitializeRoles(IRoleRepository repository)
    {
        await repository.InitializeRolesOnStartUp();
    }
}