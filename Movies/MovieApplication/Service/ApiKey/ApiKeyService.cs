using System.Net;
using System.Net.Http.Json;
using Common.Exception.HttpException;
using Microsoft.Extensions.Logging;
using MovieApplication.ReturnModel.UserApiKey;
using MovieApplication.ServiceInterface.ApiKey;
using MovieCore.ValidationResult.ApiKeyValidationResult;
using MovieInfrastructure.Setting;
using Newtonsoft.Json;
using UnauthorizedAccessException = MovieApi.CustomException.UnauthorizedAccessException;

namespace MovieApplication.Service.ApiKey;

public class ApiKeyService
    (HttpClient httpClient, ApiKeyUrlSetting authUrl, ILogger<ApiKeyService> logger) : IApiKeyService
{
    public async Task<bool> ValidateApiKeyAsync(string apiKey)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("auth/validate-api-key", apiKey);

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
                case HttpStatusCode.Unauthorized:
                    logger.LogWarning("API key is invalid thus unauthorized");
                    throw new UnauthorizedException("API key is invalid thus unauthorized.");
            }

            if (response.StatusCode
                is HttpStatusCode.BadRequest
                or HttpStatusCode.Unauthorized
                or HttpStatusCode.Forbidden)
            {
                logger.LogWarning("API key is invalid or missing");
                throw new ArgumentException("API key is invalid or missing.");
            }


            throw new Exception("Unexpected response from the authentication service.");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Unable to connect to the authentication service.");
        }
    }

    public async Task<UserApiKey?> GetUserFromApiKeyAsync(string apiKey)
    {
        var response = await httpClient.GetAsync($"{authUrl}/user-details?apiKey={apiKey}");
        if (response.IsSuccessStatusCode) return await response.Content.ReadAsAsync<UserApiKey>();

        // Handle errors or not found cases
        return null;
    }
}