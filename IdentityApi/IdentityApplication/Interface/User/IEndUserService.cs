using IdentityCore.Model.Users;

namespace IdentityApplication.Interface.User;

public interface IEndUserService
{
    Task<(string jwtToken, string refreshToken)> LoginAsync(string emailOrUsername, string password);
    Task<EndUserRegistration?> RegisterAsync(EndUserRegistration endUserRegistration);
}