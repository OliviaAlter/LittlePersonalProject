using MovieApplication.ReturnModel.UserApiKey;

namespace MovieApplication.ServiceInterface.ApiKey;

public interface IApiKeyService
{
    Task<bool> ValidateApiKeyAsync(string apiKey);
    Task<UserApiKey?> GetUserFromApiKeyAsync(string apiKey);
}