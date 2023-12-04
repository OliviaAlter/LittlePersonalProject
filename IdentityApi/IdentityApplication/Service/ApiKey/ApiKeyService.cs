using IdentityApplication.ServiceInterface.ApiKey;
using IdentityCore.Model.ObjectResponse;
using IdentityCore.RepositoryInterface.ApiKey;

namespace IdentityApplication.Service.ApiKey;

public class ApiKeyService(IApiKeyRepository apiKeyRepository) : IApiKeyService
{
    public async Task<string> GetApiKeyAsync(Guid endUserId)
    {
        return await apiKeyRepository.GetApiKeyAsync(endUserId);
    }

    public async Task<string> CreateApiKeyAsync(Guid endUserId)
    {
        return await apiKeyRepository.CreateApiKeyAsync(endUserId);
    }

    public async Task<bool> IsApiKeyValidAsync(string apiKeyId, Guid? endUserId)
    {
        return await apiKeyRepository.IsApiKeyValidAsync(apiKeyId, endUserId);
    }

    public async Task<UserApiKeyResponse?> GetUserFromApiKeyAsync(string providedApiKey)
    {
        return await apiKeyRepository.GetUserFromApiKeyAsync(providedApiKey);
    }

    public async Task<bool> RevokeApiKeyAsync(Guid endUserId)
    {
        return await apiKeyRepository.RevokeApiKeyAsync(endUserId);
    }
}