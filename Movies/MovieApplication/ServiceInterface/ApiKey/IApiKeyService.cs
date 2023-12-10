namespace MovieApplication.ServiceInterface.ApiKey;

public interface IApiKeyService
{
    Task<bool> ValidateApiKeyAsync(string apiKey);
}