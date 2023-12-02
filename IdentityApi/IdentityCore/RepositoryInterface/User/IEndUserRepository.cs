using IdentityCore.Model.Users;
using IdentityCore.RepositoryInterface.Generic;

namespace IdentityCore.RepositoryInterface.User;

public interface IEndUserRepository : IGenericRepository<EndUser>
{
    Task<EndUser> LoginAsync(string emailOrUsername);
}