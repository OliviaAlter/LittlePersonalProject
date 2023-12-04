using IdentityCore.Model.ObjectResponse;

namespace IdentityApplication.ServiceInterface.ApiKey;

public interface IApiKeyService
{
    Task<string> GetApiKeyAsync(Guid endUserId);
    Task<string> CreateApiKeyAsync(Guid endUserId);
    Task<bool> IsApiKeyValidAsync(string apiKeyId, Guid? endUserId = null);
    Task<UserApiKeyResponse?> GetUserFromApiKeyAsync(string providedApiKey);
    Task<bool> RevokeApiKeyAsync(Guid endUserId);
}