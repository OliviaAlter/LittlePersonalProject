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
        var endUserId = GetUserId();

        var apiKey = await apiKeyService.GetApiKeyAsync(endUserId.Value);

        return Ok(new
        {
            Apikey = apiKey
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateApiKey()
    {
        var endUserId = GetUserId();

        var apiKey = await apiKeyService.CreateApiKeyAsync(endUserId.Value);

        if (string.IsNullOrEmpty(apiKey))
            throw new BadHttpRequestException("API Key creation failed.");

        return Ok(new
        {
            ApiKey = apiKey
        });
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeApiKey()
    {
        var endUserId = GetUserId();

        var result = await apiKeyService.RevokeApiKeyAsync(endUserId.Value);

        if (!result)
            throw new BadHttpRequestException("API Key revocation failed.");

        return Ok();
    }

    private Guid? GetUserId()
    {
        var userId = HttpContext.GetUserId();

        if (userId is null)
            throw new BadHttpRequestException("User not found.");

        return userId;
    }
}