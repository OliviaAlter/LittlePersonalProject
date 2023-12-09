using IdentityCore.Model.ObjectResponse;

namespace IdentityApplication.ServiceInterface.ApiKey;

public interface IApiKeyService
{
    Task<string> GetApiKeyAsync(Guid accountId);
    Task<string> CreateApiKeyAsync(Guid accountId);
    Task<bool> IsApiKeyValidAsync(string apiKeyId, Guid? accountId = null);
    Task<UserApiKeyResponse?> GetUserFromApiKeyAsync(string providedApiKey);
    Task<bool> RevokeApiKeyAsync(Guid accountId);
}