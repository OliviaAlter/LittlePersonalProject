using IdentityCore.Model.Users;
using IdentityCore.RepositoryInterface.User;
using IdentityInfrastructure.Data;
using IdentityInfrastructure.DatabaseException;
using IdentityInfrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace IdentityInfrastructure.Repository.User;

public class EndUserRepository(IApplicationDbContext context) :
    GenericRepository<EndUser>(context), IEndUserRepository
{
    public async Task<EndUser> LoginAsync(string emailOrUsername)
    {
        var endUser =
            await context.Users
                .FirstOrDefaultAsync(x => x.Email == emailOrUsername
                                          || x.Username == emailOrUsername);

        if (endUser == null)
            throw new EntityNotFoundException("User not found");

        return endUser;
    }
}