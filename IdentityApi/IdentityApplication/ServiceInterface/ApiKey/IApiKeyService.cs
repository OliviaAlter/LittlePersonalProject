using Common.Exception.ObjectResponse;

namespace IdentityApplication.ServiceInterface.ApiKey;

public interface IApiKeyService
{
    Task<string> GetApiKeyAsync(Guid accountId);
    Task<string> CreateApiKeyAsync(Guid accountId);
    Task<bool> IsApiKeyValidAsync(string apiKeyId);
    Task<UserApiKeyResponse?> GetUserFromApiKeyAsync(string providedApiKey);
    Task<bool> RevokeApiKeyAsync(Guid accountId);
}