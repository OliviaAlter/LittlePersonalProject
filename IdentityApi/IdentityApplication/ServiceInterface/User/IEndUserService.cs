using IdentityCore.Model.DatabaseEntity.Users;

namespace IdentityApplication.ServiceInterface.User;

public interface IEndUserService
{
    Task<(string jwtToken, string refreshToken)> LoginAsync(string emailOrUsername, string password);
    Task<EndUserRegistration?> RegisterAsync(EndUserRegistration endUserRegistration);
}