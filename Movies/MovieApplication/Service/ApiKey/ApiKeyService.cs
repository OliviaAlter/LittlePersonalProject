using System.Net;
using System.Net.Http.Json;
using MovieApplication.ServiceInterface.ApiKey;
using MovieCore.ValidationResult.ApiKeyValidationResult;
using Newtonsoft.Json;

namespace MovieApplication.Service.ApiKey;

public class ApiKeyService(HttpClient httpClient) : IApiKeyService
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

            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new ArgumentException("API key is invalid or missing.");

            throw new Exception("Unexpected response from the authentication service.");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Unable to connect to the authentication service.");
        }
    }
}