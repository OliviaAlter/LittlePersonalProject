using Identity.IdentityExtension;
using IdentityApplication.ServiceInterface.ApiKey;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controller;

[Route("api/auth/api-key")]
[ApiController]
public class ApiKeyController(IApiKeyService apiKeyService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetApiKey()
    {
        var accountId = GetAccountIdFromToken();

        var apiKey = await apiKeyService.GetApiKeyAsync(accountId.Value);

        return Ok(new
        {
            Apikey = apiKey
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateApiKey()
    {
        var accountId = GetAccountIdFromToken();

        var apiKey = await apiKeyService.CreateApiKeyAsync(accountId.Value);

        if (string.IsNullOrEmpty(apiKey))
            throw new BadHttpRequestException("API Key creation failed.");

        return Ok(new
        {
            ApiKey = apiKey
        });
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateApiKey([FromBody] string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey)) return BadRequest("API key is required.");

        var isValid = await apiKeyService.IsApiKeyValidAsync(apiKey);

        return Ok(new
        {
            IsValid = isValid
        });
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeApiKey()
    {
        var accountId = GetAccountIdFromToken();

        var result = await apiKeyService.RevokeApiKeyAsync(accountId.Value);

        if (!result)
            throw new BadHttpRequestException("API Key revocation failed.");

        return Ok();
    }

    private Guid? GetAccountIdFromToken()
    {
        var accountId = HttpContext.GetAccountId();

        if (accountId is null)
            throw new BadHttpRequestException("Account not found.");

        return accountId;
    }
}