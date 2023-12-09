using IdentityCore.Model.DatabaseEntity.AccountModel;
using IdentityCore.Model.Token;

namespace IdentityCore.ServiceInterface.Token;

public interface ITokenService
{
    Task<string> GenerateHealthCheckTokenAsync();
    Task<(string jwtToken, string refreshToken)> GenerateTokenForUser(Account account);
    Task<(string jwtToken, string refreshToken)> RefreshTokenForUser(TokenGeneration token);
    Task<bool> IsRefreshTokenValid(string refreshToken, Guid accountId);
    Task<Guid> GetUserIdFromToken(string token);
}