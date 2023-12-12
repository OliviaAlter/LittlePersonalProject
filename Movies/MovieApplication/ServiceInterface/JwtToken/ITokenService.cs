using System.Security.Claims;

namespace MovieApplication.ServiceInterface.JwtToken;

public interface ITokenService
{
    IEnumerable<Claim>? GetClaimsFromToken(string token);
    Task<bool> IsRefreshTokenValid(string refreshToken);
}