using MovieApplication.Service.ApiKey;
using MovieApplication.Service.JwtToken;
using MovieApplication.ServiceInterface.ApiKey;
using MovieApplication.ServiceInterface.JwtToken;

namespace MovieApi.ServiceExtension;

public static class InjectServiceExtension
{
    public static void AddInjectService(this IServiceCollection services)
    {
        services.AddScoped<IApiKeyService, ApiKeyService>();

        services.AddScoped<ITokenService, TokenService>();
    }
}