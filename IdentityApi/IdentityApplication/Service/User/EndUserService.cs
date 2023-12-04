using IdentityApplication.Extension;
using IdentityApplication.ServiceInterface.User;
using IdentityCore.BusinessRuleInterface.EndUser;
using IdentityCore.Enum;
using IdentityCore.Model.DatabaseEntity.Users;
using IdentityCore.RepositoryInterface.Role;
using IdentityCore.RepositoryInterface.User;
using IdentityCore.ServiceInterface.Audit;
using IdentityCore.ServiceInterface.Password;
using IdentityCore.ServiceInterface.Token;

namespace IdentityApplication.Service.User;

public class EndUserService(IEndUserRepository endUserRepository, IRoleRepository roleRepository,
    IPasswordHashingService passwordService, ITokenService tokenService,
    IAuditService auditService, IEndUserBusinessRuleService businessRule) : IEndUserService
{
    public async Task<(string jwtToken, string refreshToken)> LoginAsync(string emailOrUsername, string password)
    {
        var endUser = await endUserRepository.LoginAsync(emailOrUsername);

        if (!passwordService.VerifyPasswordHash(password, endUser.PasswordHash, endUser.Salt))
            throw new Exception("Password or username/email is incorrect");

        var (jwtToken, refreshToken) = await tokenService.GenerateTokenForUser(endUser);

        await auditService
            .LogToDatabaseAsync(endUser.EndUserId.ToString(),
                "Logged in at " + DateTime.UtcNow.FormatDateTime());

        return (jwtToken, refreshToken);
    }

    public async Task<EndUserRegistration?> RegisterAsync(EndUserRegistration user)
    {
        if (!await businessRule.IsUsernameUniqueAsync(user.Username))
            throw new Exception("Username must be unique.");

        if (!businessRule.IsValidPassword(user.Password))
            throw new Exception("Password does not meet the required policy.");

        try
        {
            var (hash, salt) = await passwordService.CreatePasswordHashAsync(user.Password);

            var role = await roleRepository.GetOrCreateRoleAsync(UserRole.NormalUser);

            var registerUser = new EndUser
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
            await endUserRepository.AddAsync(registerUser);

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