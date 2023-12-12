using Common.Exception.DatabaseException.CommonDatabaseException;
using IdentityCore.Enum;
using IdentityCore.Model.DatabaseEntity.RoleModel;
using IdentityCore.RepositoryInterface.Role;
using IdentityInfrastructure.Data;
using IdentityInfrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityInfrastructure.Repository.Role;

public class RoleRepository(IAuthIdentityDbContext context, ILogger<RoleRepository> logger)
    : GenericRepository<AccountRole>(context), IRoleRepository
{
    public async Task InitializeRolesOnStartUp()
    {
        await using var transaction = await context.BeginTransactionAsync();
        try
        {
            var existingRoles = await GetExistingRoles();
            var rolesToCreate = GetRolesToCreate(existingRoles);

            if (rolesToCreate.Count != 0)
            {
                await CreateRoles(rolesToCreate);
                await context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }
        catch (DatabaseOperationException ex) // Catch a more general exception, or multiple specific exceptions
        {
            // Log the exception if necessary
            await transaction.RollbackAsync();
            throw new DatabaseOperationException("Batch insert of roles failed.", ex);
        }
    }

    public async Task<Guid> GetOrCreateRoleAsync(AccountRoleEnum rolesEnum)
    {
        var roleName = GetRoleNameAsync(rolesEnum);

        if (roleName is null) throw new ArgumentException($"Invalid role: {rolesEnum}", nameof(rolesEnum));

        var role = await context.Roles
            .FirstOrDefaultAsync(x =>
                string.Equals(x.RoleName, roleName, StringComparison.OrdinalIgnoreCase));

        if (role is not null) return role.RoleId;

        role = new AccountRole
        {
            RoleId = Guid.NewGuid(),
            RoleName = roleName
        };

        await context.Roles.AddAsync(role);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create a new role: {RoleName}", roleName);
            throw new DatabaseOperationException("Error occurred while creating a new role.", ex);
        }

        return role.RoleId;
    }

    private static List<AccountRole> GetRolesToCreate(IEnumerable<AccountRole> existingRoles)
    {
        return Enum.GetNames(typeof(AccountRoleEnum))
            .Where(role => !existingRoles.Any(r => r.RoleName.Equals(role, StringComparison.OrdinalIgnoreCase)))
            .Select(role => new AccountRole { RoleId = Guid.NewGuid(), RoleName = role })
            .ToList();
    }

    private async Task CreateRoles(IEnumerable<AccountRole> rolesToCreate)
    {
        await context.Roles.AddRangeAsync(rolesToCreate);
    }

    private async Task<List<AccountRole>> GetExistingRoles()
    {
        return await context.Roles.ToListAsync();
    }

    private static string GetRoleNameAsync(AccountRoleEnum rolesEnum)
    {
        var roleName = rolesEnum switch
        {
            AccountRoleEnum.Admin => nameof(AccountRoleEnum.Admin),
            AccountRoleEnum.NormalUser => nameof(AccountRoleEnum.NormalUser),
            AccountRoleEnum.Moderator => nameof(AccountRoleEnum.Moderator),
            AccountRoleEnum.SuperAdmin => nameof(AccountRoleEnum.SuperAdmin),
            _ => null
        };

        if (roleName is null) throw new ArgumentException($"Invalid role: {rolesEnum}", nameof(rolesEnum));

        return roleName;
    }
}