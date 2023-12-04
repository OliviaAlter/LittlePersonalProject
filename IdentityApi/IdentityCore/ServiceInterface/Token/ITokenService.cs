using IdentityCore.Model.DatabaseEntity.Token;
using IdentityCore.Model.DatabaseEntity.Users;

namespace IdentityCore.ServiceInterface.Token;

public interface ITokenService
{
    Task<string> GenerateHealthCheckTokenAsync();
    Task<(string jwtToken, string refreshToken)> GenerateTokenForUser(EndUser user);
    Task<(string jwtToken, string refreshToken)> RefreshTokenForUser(TokenGeneration token);
    Task<bool> IsRefreshTokenValid(string refreshToken, Guid userId);
    Task<Guid> GetUserIdFromToken(string token);
}