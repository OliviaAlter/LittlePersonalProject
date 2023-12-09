using IdentityCore.BusinessRuleInterface.EndUser;
using IdentityCore.Model.DatabaseEntity.AccountModel;
using IdentityCore.RepositoryInterface.Generic;

namespace IdentityApplication.BusinessRule.User;

public class EndUserBusinessRuleService(IGenericRepository<Account> userRepository) : IEndUserBusinessRuleService
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