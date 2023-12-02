using IdentityCore.ServiceInterface.Token;
using IdentityInfrastructure.Service.Token;
using IdentityInfrastructure.Setting;

namespace Identity.ServiceExtension;

public static class JwtTokenServiceExtension
{
    public static void AddJwtTokenService(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();

        configuration.Bind("JwtToken", jwtSettings);

        services.AddSingleton(jwtSettings);

        services.AddScoped<ITokenService, TokenService>();
    }
}