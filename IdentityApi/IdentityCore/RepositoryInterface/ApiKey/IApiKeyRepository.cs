using Common.Exception.ObjectResponse;

namespace IdentityCore.RepositoryInterface.ApiKey;

public interface IApiKeyRepository
{
    Task<string> GetApiKeyAsync(Guid accountId);
    Task<string> CreateApiKeyAsync(Guid accountId);
    Task<bool> IsApiKeyValidAsync(string apiKeyId);
    Task<bool> RevokeApiKeyAsync(Guid accountId);
    Task<UserApiKeyResponse?> GetUserFromApiKeyAsync(string providedApiKey);
}