using Microsoft.AspNetCore.Authentication;

namespace MovieBookingCore.Options.ApiKey;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; } = "X-API-KEY";
}