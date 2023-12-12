using System.IdentityModel.Tokens.Jwt;
using System.Text;
using IdentityCore.ServiceInterface.Token;
using IdentityInfrastructure.Service.Token;
using IdentityInfrastructure.Setting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Identity.ServiceExtension;

public static class JwtTokenServiceExtension
{
    public static void AddJwtTokenService(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();

        configuration.Bind("JwtToken", jwtSettings);

        services.AddSingleton(jwtSettings);

        services.AddScoped<ITokenService, TokenService>();

        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                    (configuration["JwtToken:NotTokenKeyForSureSourceTrustMeDude"]
                     ?? throw new InvalidOperationException("Jwt secret is missing"))),
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtToken:Issuer"],
                    ValidAudiences = configuration.GetSection("JwtToken:Audiences").Get<string[]>()
                };
            });

        /*
        services.AddAuthorization(x =>
        {
            x.AddPolicy(AuthConstants.AdminUserPolicyName,
                p => p.RequireClaim(AuthConstants.AdminUserClaimName, "true"));

            x.AddPolicy(AuthConstants.TrustedMemberPolicyName,
                p => p.RequireAssertion(c =>
                    c.Account.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" }) ||
                    c.Account.HasClaim(m => m is { Type: AuthConstants.TrustedMemberClaimName, Value: "true" })));
        });
        */
    }
}