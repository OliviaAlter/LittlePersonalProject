using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieApplication.ServiceInterface.ApiKey;
using MovieCore.Options.ApiKey;

namespace MovieApplication.Handler;

public class ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IApiKeyService apiKeyService)
    : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder, clock)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var providedApiKey = string.Empty;

        var apiKeyHeaderName = Options.HeaderName;

        if (!Request.Headers.TryGetValue(apiKeyHeaderName, out var apiKeyHeaderValues))
            return AuthenticateResult.Fail("API Key is missing from the headers.");

        if (apiKeyHeaderValues.Count > 0) providedApiKey = apiKeyHeaderValues[0];

        if (apiKeyHeaderValues.Count == 0 || providedApiKey is null || string.IsNullOrWhiteSpace(providedApiKey))
            return AuthenticateResult.Fail("API Key is empty.");

        var isValidApiKey = await apiKeyService.ValidateApiKeyAsync(providedApiKey);

        if (!isValidApiKey) return AuthenticateResult.Fail("Invalid API Key provided.");

        var account = await apiKeyService.GetUserFromApiKeyAsync(providedApiKey);

        if (account is null)
            return AuthenticateResult.Fail("No account associated with this API key.");

        var claims = new[]
        {
            new Claim(ClaimTypes.PrimarySid, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Name, account.Username),
            new Claim("AccountId", account.AccountId.ToString())
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);

        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}