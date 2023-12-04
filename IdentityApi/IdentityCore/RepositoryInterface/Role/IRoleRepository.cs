using IdentityCore.Enum;
using IdentityCore.Model.DatabaseEntity.UserRole;
using IdentityCore.RepositoryInterface.Generic;

namespace IdentityCore.RepositoryInterface.Role;

public interface IRoleRepository : IGenericRepository<Roles>
{
    Task InitializeRolesOnStartUp();
    Task<Guid> GetOrCreateRoleAsync(UserRole roles);
}