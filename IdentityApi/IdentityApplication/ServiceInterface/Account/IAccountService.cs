using IdentityCore.ModelValidation.UserModel;

namespace IdentityApplication.ServiceInterface.Account;

public interface IAccountService
{
    Task<(string jwtToken, string refreshToken)> LoginAsync(string emailOrUsername, string password);
    Task<UserRegistration?> RegisterAsync(UserRegistration userRegistration);
}