using IdentityCore.Model.DatabaseEntity.AccountModel;
using IdentityCore.RepositoryInterface.Generic;

namespace IdentityCore.RepositoryInterface.User;

public interface IUserRepository : IGenericRepository<Account>
{
    Task<Account> LoginAsync(string emailOrUsername);
}