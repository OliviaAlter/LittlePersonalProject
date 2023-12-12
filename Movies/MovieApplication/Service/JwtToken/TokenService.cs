using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Common.Exception.Setting.Option.ApiKeyUrl;
using MovieApplication.ServiceInterface.JwtToken;

namespace MovieApplication.Service.JwtToken;

public class TokenService(HttpClient httpClient, ApiKeyUrlSetting authUrl) : ITokenService
{
    public IEnumerable<Claim>? GetClaimsFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        return jwtToken?.Claims;
    }

    public async Task<bool> IsRefreshTokenValid(string refreshToken)
    {
        var response = await httpClient.PostAsJsonAsync($"{authUrl.ApiKeyUrl}/validate-token", refreshToken);
        return response.IsSuccessStatusCode;
    }
}