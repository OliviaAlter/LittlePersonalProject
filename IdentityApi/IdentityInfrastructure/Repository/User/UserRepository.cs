using Common.Exception.DatabaseException.CommonDatabaseException;
using IdentityCore.Model.DatabaseEntity.AccountModel;
using IdentityCore.RepositoryInterface.User;
using IdentityInfrastructure.Data;
using IdentityInfrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace IdentityInfrastructure.Repository.User;

public class UserRepository(IAuthIdentityDbContext context) :
    GenericRepository<Account>(context), IUserRepository
{
    public async Task<Account> LoginAsync(string emailOrUsername)
    {
        var user =
            await context.Users
                .FirstOrDefaultAsync(x => x.Email == emailOrUsername
                                          || x.Username == emailOrUsername);

        if (user is null)
            throw new EntityNotFoundException("User not found");

        return user;
    }
}