using IdentityCore.Model.ObjectResponse;

namespace IdentityCore.RepositoryInterface.ApiKey;

public interface IApiKeyRepository
{
    Task<string> GetApiKeyAsync(Guid endUserId);
    Task<string> CreateApiKeyAsync(Guid endUserId);
    Task<bool> IsApiKeyValidAsync(string apiKeyId, Guid? endUserId = null);
    Task<bool> RevokeApiKeyAsync(Guid endUserId);
    Task<UserApiKeyResponse?> GetUserFromApiKeyAsync(string providedApiKey);
}