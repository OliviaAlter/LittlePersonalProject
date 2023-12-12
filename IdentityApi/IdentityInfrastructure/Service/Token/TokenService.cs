using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityCore.Model.DatabaseEntity.AccountModel;
using IdentityCore.Model.Token;
using IdentityCore.RepositoryInterface.Account;
using IdentityCore.ServiceInterface.Token;
using IdentityInfrastructure.Setting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityInfrastructure.Service.Token;

public class TokenService
    (JwtSettings jwtSettings, IAccountRepository repository) : ITokenService
{
    private const int TokenExpiryHours = 12;

    public async Task<string> GenerateHealthCheckTokenAsync()
    {
        try
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, "HealthCheckUser")
            };

            return await Task.FromResult(CreateJwtToken(claims));
        }
        catch
        {
            // Consider logging the exception here
            throw new InvalidOperationException("Token generation failed in service.");
        }
    }

    public async Task<(string jwtToken, string refreshToken)> GenerateTokenForUser(Account account)
    {
        var claims = GenerateClaimsForUser(account);

        var jwtToken = CreateJwtToken(claims);

        var refreshToken = GenerateRefreshToken();

        account.RefreshToken = refreshToken;
        account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await repository.UpdatePartialAsync(account);

        return (jwtToken, refreshToken);
    }

    public async Task<(string jwtToken, string refreshToken)> RefreshTokenForUser(TokenGeneration request)
    {
        var account = await GetUserFromToken(request.Token);

        // Validate the refresh token
        if (!await IsRefreshTokenValid(request.RefreshToken, account.AccountId))
            throw new InvalidOperationException("Invalid refresh token.");

        var claims = GenerateClaimsForUser(account);

        var newJwtToken = CreateJwtToken(claims);

        var newRefreshToken = GenerateRefreshToken();

        // Update the account with the new refresh token
        account.RefreshToken = newRefreshToken;

        account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set new expiry

        await repository.UpdatePartialAsync(account);

        return (newJwtToken, newRefreshToken); // Return the new JWT token
    }

    public async Task<bool> IsRefreshTokenValid(string refreshToken, Guid accountId)
    {
        var localUserId = accountId;

        var account = await repository.FindAsync(x => x.AccountId == localUserId);

        if (account is null || account.RefreshToken != refreshToken ||
            account.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new InvalidOperationException("Invalid client request");

        return account.RefreshToken == refreshToken && account.RefreshTokenExpiryTime > DateTime.UtcNow;
    }

    public async Task<Guid> GetUserIdFromToken(string token)
    {
        var userIdClaim = await GetUserIdFromClaims(token);

        if (Guid.TryParse(userIdClaim, out var accountId))
            return await Task.FromResult(accountId);

        throw new ArgumentException("Invalid token");
    }

    private async Task<Account> GetUserFromToken(string token)
    {
        var userIdClaim = await GetUserIdFromClaims(token);

        if (!Guid.TryParse(userIdClaim, out var accountId))
            throw new InvalidOperationException("Invalid token");

        var userToFind = accountId;

        var account = await repository.FindAsync(x => x.AccountId == userToFind);

        if (account is null)
            throw new InvalidOperationException("Invalid token");

        return account;
    }

    private async Task<string> GetUserIdFromClaims(string token)
    {
        var principal = GetPrincipalFromToken(token);

        var userIdClaim = principal.Identities.FirstOrDefault()?
            .Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;

        if (userIdClaim is null)
            throw new ArgumentException("Invalid token");

        return await Task.FromResult(userIdClaim);
    }

    private string CreateJwtToken(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityKey = GetSecurityKey(); // Abstracted method to get security key

        var credential = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(TokenExpiryHours), // Defined as a constant or configuration
            Issuer = jwtSettings.Issuer,
            Audience = string.Join(" ", jwtSettings.Audiences),
            SigningCredentials = credential
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private SymmetricSecurityKey GetSecurityKey()
    {
        return new SymmetricSecurityKey(
            Encoding.UTF8
                .GetBytes(jwtSettings.NotTokenKeyForSureSourceTrustMeDude
                          ?? throw new InvalidOperationException("Jwt secret is missing")));
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityKey = GetSecurityKey();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is JwtSecurityToken jwtSecurityToken
                && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
                    StringComparison.InvariantCultureIgnoreCase))
                return principal;

            throw new SecurityTokenException("Invalid token");
        }
        catch
        {
            throw new SecurityTokenException("Token validation failed");
        }
    }

    private static List<Claim> GenerateClaimsForUser(Account request)
    {
        return new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, request.Email),
            new(JwtRegisteredClaimNames.Name, request.Username),
            new("AccountId", request.AccountId.ToString())
        };
    }
}