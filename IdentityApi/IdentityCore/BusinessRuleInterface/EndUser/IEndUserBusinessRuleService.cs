namespace IdentityCore.BusinessRuleInterface.EndUser;

public interface IEndUserBusinessRuleService
{
    Task<bool> IsUsernameUniqueAsync(string username);
    bool IsValidPassword(string password);
}