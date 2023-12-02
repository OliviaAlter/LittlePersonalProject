using IdentityCore.Enum;
using IdentityCore.Model.UserRole;
using IdentityCore.RepositoryInterface.Role;
using IdentityInfrastructure.Data;
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
            var existingRoles = await context.Roles.ToListAsync();

            foreach (var role in Enum.GetNames(typeof(UserRole)))
                // Perform the comparison in memory using LINQ to Objects
                if (!existingRoles.Any(r => r.RoleName.Equals(role, StringComparison.OrdinalIgnoreCase)))
                    context.Roles.Add(new Roles
                    {
                        RolesId = Guid.NewGuid(),
                        RoleName = role
                    });

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            // Optionally log the exception
            await transaction.RollbackAsync();
            throw; // Rethrow the exception to handle it further up the call stack, if necessary
        }
    }

    public async Task<Guid> GetOrCreateRoleAsync(UserRole roles)
    {
        // Convert enum to string
        var roleName = Enum.GetName(typeof(UserRole), roles);

        // Check if role exists, otherwise create a new one
        var role = await context.Roles
            .FirstOrDefaultAsync(x =>
                string.Equals(x.RoleName, roleName,
                    StringComparison.CurrentCultureIgnoreCase));

        if (role is not null)
            return role.RolesId;

        role = new Roles
        {
            RolesId = Guid.NewGuid(),
            RoleName = roleName ?? throw new InvalidOperationException("Role name is null")
        };

        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();

        return role.RolesId;
    }
}