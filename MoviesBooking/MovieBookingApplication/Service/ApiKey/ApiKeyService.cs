namespace Application.Service.ApiKey;

public class ApiKeyService(IApiKeyRepository apiKeyRepository) : IApiKeyService
{
    public async Task<string> GetApiKeyAsync(Guid accountId)
    {
        return await apiKeyRepository.GetApiKeyAsync(accountId);
    }

    public async Task<string> CreateApiKeyAsync(Guid accountId)
    {
        return await apiKeyRepository.CreateApiKeyAsync(accountId);
    }

    public async Task<bool> IsApiKeyValidAsync(string apiKeyId, Guid? accountId)
    {
        return await apiKeyRepository.IsApiKeyValidAsync(apiKeyId, accountId);
    }

    public async Task<UserApiKeyResponse?> GetUserFromApiKeyAsync(string providedApiKey)
    {
        return await apiKeyRepository.GetUserFromApiKeyAsync(providedApiKey);
    }

    public async Task<bool> RevokeApiKeyAsync(Guid accountId)
    {
        return await apiKeyRepository.RevokeApiKeyAsync(accountId);
    }
}