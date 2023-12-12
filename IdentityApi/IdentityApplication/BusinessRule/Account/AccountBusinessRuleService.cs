using IdentityCore.BusinessRuleInterface.Account;
using IdentityCore.RepositoryInterface.Generic;

namespace IdentityApplication.BusinessRule.Account;

public class AccountBusinessRuleService(
    IGenericRepository<IdentityCore.Model.DatabaseEntity.AccountModel.Account> userRepository) :
    IAccountBusinessRuleService
{
    public async Task<bool> IsUsernameUniqueAsync(string username)
    {
        return !await userRepository
            .ExistsAsync(u
                => string.Equals(u.Username, username,
                    StringComparison.CurrentCultureIgnoreCase));
    }

    public bool IsValidPassword(string password)
    {
        // Example password policy: Minimum 8 characters max at 50, at least one letter and one number
        return password.Length is <= 50 and >= 8
               && password.Any(char.IsLetter) && password.Any(char.IsDigit);
    }
}