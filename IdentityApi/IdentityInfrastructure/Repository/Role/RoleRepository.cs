using IdentityCore.Enum;
using IdentityCore.Model.DatabaseEntity.UserRole;
using IdentityCore.RepositoryInterface.Role;
using IdentityInfrastructure.Data;
using IdentityInfrastructure.DatabaseException;
using IdentityInfrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace IdentityInfrastructure.Repository.Role;

public class RoleRepository(IApplicationDbContext context)
    : GenericRepository<Roles>(context), IRoleRepository
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

    public async Task<Guid> GetOrCreateRoleAsync(UserRole roles)
    {
        var roleName = GetRoleNameAsync(roles);

        if (roleName is null) throw new ArgumentException($"Invalid role: {roles}", nameof(roles));

        var role = await context.Roles
            .FirstOrDefaultAsync(x =>
                string.Equals(x.RoleName, roleName, StringComparison.OrdinalIgnoreCase));

        if (role is not null) return role.RolesId;

        role = new Roles
        {
            RolesId = Guid.NewGuid(),
            RoleName = roleName
        };

        await context.Roles.AddAsync(role);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Consider logging the exception
            // Log.Error(ex, "Failed to create a new role: {RoleName}", roleName);
            throw new DatabaseOperationException("Error occurred while creating a new role.", ex);
        }

        return role.RolesId;
    }

    private static List<Roles> GetRolesToCreate(IEnumerable<Roles> existingRoles)
    {
        return Enum.GetNames(typeof(UserRole))
            .Where(role => !existingRoles.Any(r => r.RoleName.Equals(role, StringComparison.OrdinalIgnoreCase)))
            .Select(role => new Roles { RolesId = Guid.NewGuid(), RoleName = role })
            .ToList();
    }

    private async Task CreateRoles(IEnumerable<Roles> rolesToCreate)
    {
        await context.Roles.AddRangeAsync(rolesToCreate);
    }

    private async Task<List<Roles>> GetExistingRoles()
    {
        return await context.Roles.ToListAsync();
    }

    private static string GetRoleNameAsync(UserRole roles)
    {
        var roleName = roles switch
        {
            UserRole.Admin => nameof(UserRole.Admin),
            UserRole.NormalUser => nameof(UserRole.NormalUser),
            UserRole.Moderator => nameof(UserRole.Moderator),
            UserRole.SuperAdmin => nameof(UserRole.SuperAdmin),
            _ => null
        };

        if (roleName is null) throw new ArgumentException($"Invalid role: {roles}", nameof(roles));

        return roleName;
    }
}