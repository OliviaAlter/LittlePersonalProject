using Microsoft.AspNetCore.Authentication;

namespace IdentityCore.Options.ApiKey;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; } = "X-API-KEY";
}