using Common.Exception.ObjectResponse;

namespace MovieBookingApplication.ServiceInterface.ApiKey;

public interface IApiKeyService
{
    Task<bool> ValidateApiKeyAsync(string apiKey);
    Task<UserApiKeyResponse?> GetUserFromApiKeyAsync(string providedApiKey);
}