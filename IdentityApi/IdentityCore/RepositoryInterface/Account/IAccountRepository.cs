using IdentityCore.RepositoryInterface.Generic;

namespace IdentityCore.RepositoryInterface.Account;

public interface IAccountRepository : IGenericRepository<Model.DatabaseEntity.AccountModel.Account>
{
    Task<Model.DatabaseEntity.AccountModel.Account> LoginAsync(string emailOrUsername);
}