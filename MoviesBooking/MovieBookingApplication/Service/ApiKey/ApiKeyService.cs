using System.Net;
using System.Net.Http.Json;
using Common.Exception.HttpException;
using Common.Exception.ObjectResponse;
using Common.Exception.Setting.Option.ApiKeyUrl;
using Microsoft.Extensions.Logging;
using MovieBookingApplication.ServiceInterface.ApiKey;
using MovieCore.ValidationResult.ApiKeyValidationResult;
using Newtonsoft.Json;

namespace MovieBookingApplication.Service.ApiKey;

public class ApiKeyService
    (HttpClient httpClient, ApiKeyUrlSetting authUrl, ILogger<ApiKeyService> logger) : IApiKeyService
{
    public async Task<bool> ValidateApiKeyAsync(string apiKey)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{authUrl.ApiKeyUrl}/validate", apiKey);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiKeyValidationResult>(content);

                if (result is null)
                    throw new Exception("Unable to deserialize response from the authentication service.");

                return result.IsValid;
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    logger.LogWarning("API key is invalid or missing");
                    throw new BadRequestException("API key is invalid or missing.");
                case HttpStatusCode.Forbidden:
                    logger.LogWarning("API key is valid but forbidden");
                    throw new ForbiddenAccessException("API key is valid but forbidden.");
                case HttpStatusCode.Unauthorized:
                    logger.LogWarning("API key is invalid thus unauthorized");
                    throw new UnauthorizedException("API key is invalid thus unauthorized.");
            }

            logger.LogWarning("Unexpected response from the authentication service");
            throw new Exception("Unexpected response from the authentication service.");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Unable to connect to the authentication service.");
        }
    }

    public async Task<UserApiKeyResponse?> GetUserFromApiKeyAsync(string providedApiKey)
    {
        try
        {
            var response = await httpClient.GetAsync($"{authUrl.ApiKeyUrl}/account-details?apiKey={providedApiKey}");

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Authentication API responded with status code: {StatusCode}", response.StatusCode);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        logger.LogWarning("API key is invalid or missing");
                        throw new BadRequestException("API key is invalid or missing.");
                    case HttpStatusCode.Forbidden:
                        logger.LogWarning("API key is valid but forbidden");
                        throw new ForbiddenAccessException("API key is valid but forbidden.");
                    case HttpStatusCode.Unauthorized:
                        logger.LogWarning("API key is invalid thus unauthorized");
                        throw new UnauthorizedException("API key is invalid thus unauthorized.");
                }
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserApiKeyResponse>(content);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error occurred while calling the authentication API");
            throw;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Error occurred while deserializing the response from the authentication API");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred");
            throw;
        }
    }
}