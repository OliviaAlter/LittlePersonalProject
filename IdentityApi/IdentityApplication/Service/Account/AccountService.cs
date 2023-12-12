using IdentityApplication.Extension;
using IdentityApplication.ServiceInterface.Account;
using IdentityCore.BusinessRuleInterface.Account;
using IdentityCore.Enum;
using IdentityCore.ModelValidation.UserModel;
using IdentityCore.RepositoryInterface.Account;
using IdentityCore.RepositoryInterface.Role;
using IdentityCore.ServiceInterface.Audit;
using IdentityCore.ServiceInterface.Password;
using IdentityCore.ServiceInterface.Token;

namespace IdentityApplication.Service.Account;

public class AccountService(IAccountRepository accountRepository, IRoleRepository roleRepository,
    IPasswordHashingService passwordService, ITokenService tokenService,
    IAuditService auditService, IAccountBusinessRuleService businessRule) : IAccountService
{
    public async Task<(string jwtToken, string refreshToken)> LoginAsync(string emailOrUsername, string password)
    {
        var account = await accountRepository.LoginAsync(emailOrUsername);

        if (!passwordService.VerifyPasswordHash(password, account.PasswordHash, account.Salt))
            throw new Exception("Password or username/email is incorrect");

        var (jwtToken, refreshToken) = await tokenService.GenerateTokenForUser(account);

        await auditService
            .LogToDatabaseAsync(account.AccountId.ToString(),
                "Logged in at " + DateTime.UtcNow.FormatDateTime());

        return (jwtToken, refreshToken);
    }

    public async Task<UserRegistration?> RegisterAsync(UserRegistration account)
    {
        if (!await businessRule.IsUsernameUniqueAsync(account.Username))
            throw new Exception("Username must be unique.");

        if (!businessRule.IsValidPassword(account.Password))
            throw new Exception("Password does not meet the required policy.");

        try
        {
            var (hash, salt) = await passwordService.CreatePasswordHashAsync(account.Password);

            var role = await roleRepository.GetOrCreateRoleAsync(AccountRoleEnum.NormalUser);

            var registerUser = new IdentityCore.Model.DatabaseEntity.AccountModel.Account
            {
                Username = account.Username,
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                PhoneNumber = account.PhoneNumber,
                PasswordHash = hash,
                Salt = salt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                RolesId = role
            };

            // Continue with adding the account
            await accountRepository.AddAsync(registerUser);

            await auditService
                .LogToDatabaseAsync(account.Username,
                    "Registered at " + DateTime.UtcNow.FormatDateTime());
        }
        catch (Exception e)
        {
            await auditService
                .LogToDatabaseAsync(account.Username,
                    "Error while creating password hash: "
                    + e.Message + " at "
                    + DateTime.UtcNow.FormatDateTime());

            throw new Exception("Error while creating password hash.");
        }

        return account;
    }
}