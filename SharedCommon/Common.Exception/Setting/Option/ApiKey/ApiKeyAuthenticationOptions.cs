using Microsoft.AspNetCore.Authentication;

namespace Common.Exception.Setting.Option.ApiKey;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; } = "X-API-KEY";
}