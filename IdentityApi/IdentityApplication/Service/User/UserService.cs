using IdentityApplication.Extension;
using IdentityApplication.ServiceInterface.User;
using IdentityCore.BusinessRuleInterface.EndUser;
using IdentityCore.Enum;
using IdentityCore.Model.DatabaseEntity.AccountModel;
using IdentityCore.ModelValidation.UserModel;
using IdentityCore.RepositoryInterface.Role;
using IdentityCore.RepositoryInterface.User;
using IdentityCore.ServiceInterface.Audit;
using IdentityCore.ServiceInterface.Password;
using IdentityCore.ServiceInterface.Token;

namespace IdentityApplication.Service.User;

public class UserService(IUserRepository userRepository, IRoleRepository roleRepository,
    IPasswordHashingService passwordService, ITokenService tokenService,
    IAuditService auditService, IEndUserBusinessRuleService businessRule) : IEndUserService
{
    public async Task<(string jwtToken, string refreshToken)> LoginAsync(string emailOrUsername, string password)
    {
        var user = await userRepository.LoginAsync(emailOrUsername);

        if (!passwordService.VerifyPasswordHash(password, user.PasswordHash, user.Salt))
            throw new Exception("Password or username/email is incorrect");

        var (jwtToken, refreshToken) = await tokenService.GenerateTokenForUser(user);

        await auditService
            .LogToDatabaseAsync(user.UserId.ToString(),
                "Logged in at " + DateTime.UtcNow.FormatDateTime());

        return (jwtToken, refreshToken);
    }

    public async Task<UserRegistration?> RegisterAsync(UserRegistration user)
    {
        if (!await businessRule.IsUsernameUniqueAsync(user.Username))
            throw new Exception("Username must be unique.");

        if (!businessRule.IsValidPassword(user.Password))
            throw new Exception("Password does not meet the required policy.");

        try
        {
            var (hash, salt) = await passwordService.CreatePasswordHashAsync(user.Password);

            var role = await roleRepository.GetOrCreateRoleAsync(UserRoleEnum.NormalUser);

            var registerUser = new Account()
            {
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PasswordHash = hash,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                RolesId = role
            };

            // Continue with adding the user
            await userRepository.AddAsync(registerUser);

            await auditService
                .LogToDatabaseAsync(user.Username,
                    "Registered at " + DateTime.UtcNow.FormatDateTime());
        }
        catch (Exception e)
        {
            await auditService
                .LogToDatabaseAsync(user.Username,
                    "Error while creating password hash: "
                    + e.Message + " at "
                    + DateTime.UtcNow.FormatDateTime());

            throw new Exception("Error while creating password hash.");
        }

        return user;
    }
}