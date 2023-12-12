namespace IdentityCore.BusinessRuleInterface.Account;

public interface IAccountBusinessRuleService
{
    Task<bool> IsUsernameUniqueAsync(string username);
    bool IsValidPassword(string password);
}