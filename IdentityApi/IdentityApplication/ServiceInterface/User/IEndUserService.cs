using IdentityCore.ModelValidation.UserModel;

namespace IdentityApplication.ServiceInterface.User;

public interface IEndUserService
{
    Task<(string jwtToken, string refreshToken)> LoginAsync(string emailOrUsername, string password);
    Task<UserRegistration?> RegisterAsync(UserRegistration userRegistration);
}