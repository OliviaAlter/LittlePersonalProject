using Common.Exception.DatabaseException.CommonDatabaseException;
using IdentityCore.RepositoryInterface.Account;
using IdentityInfrastructure.Data;
using IdentityInfrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace IdentityInfrastructure.Repository.Account;

public class AccountRepository(IAuthIdentityDbContext context) :
    GenericRepository<IdentityCore.Model.DatabaseEntity.AccountModel.Account>(context), IAccountRepository
{
    public async Task<IdentityCore.Model.DatabaseEntity.AccountModel.Account> LoginAsync(string emailOrUsername)
    {
        var account =
            await context.Accounts
                .FirstOrDefaultAsync(x => x.Email == emailOrUsername
                                          || x.Username == emailOrUsername);

        if (account is null)
            throw new EntityNotFoundException("Account not found");

        return account;
    }
}