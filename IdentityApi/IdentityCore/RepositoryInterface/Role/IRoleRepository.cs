using IdentityCore.Enum;
using IdentityCore.Model.DatabaseEntity.RoleModel;
using IdentityCore.RepositoryInterface.Generic;

namespace IdentityCore.RepositoryInterface.Role;

public interface IRoleRepository : IGenericRepository<AccountRole>
{
    Task InitializeRolesOnStartUp();
    Task<Guid> GetOrCreateRoleAsync(AccountRoleEnum rolesEnum);
}