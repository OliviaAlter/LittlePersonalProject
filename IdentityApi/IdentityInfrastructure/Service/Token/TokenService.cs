using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityCore.Model.DatabaseEntity.AccountModel;
using IdentityCore.Model.Token;
using IdentityCore.RepositoryInterface.User;
using IdentityCore.ServiceInterface.Token;
using IdentityInfrastructure.Setting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityInfrastructure.Service.Token;

public class TokenService
    (JwtSettings jwtSettings, IUserRepository repository) : ITokenService
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
        var user = await GetUserFromToken(request.Token);

        // Validate the refresh token
        if (!await IsRefreshTokenValid(request.RefreshToken, user.UserId))
            throw new InvalidOperationException("Invalid refresh token.");

        var claims = GenerateClaimsForUser(user);

        var newJwtToken = CreateJwtToken(claims);

        var newRefreshToken = GenerateRefreshToken();

        // Update the user with the new refresh token
        user.RefreshToken = newRefreshToken;

        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set new expiry

        await repository.UpdatePartialAsync(user);

        return (newJwtToken, newRefreshToken); // Return the new JWT token
    }

    public async Task<bool> IsRefreshTokenValid(string refreshToken, Guid accountId)
    {
        var localUserId = accountId;

        var user = await repository.FindAsync(x => x.UserId == localUserId);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new InvalidOperationException("Invalid client request");

        return user.RefreshToken == refreshToken && user.RefreshTokenExpiryTime > DateTime.UtcNow;
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

        var user = await repository.FindAsync(x => x.UserId == userToFind);

        if (user is null)
            throw new InvalidOperationException("Invalid token");

        return user;
    }

    private async Task<string> GetUserIdFromClaims(string token)
    {
        var principal = GetPrincipalFromToken(token);

        var userIdClaim = principal.Identities.FirstOrDefault()?
            .Claims.FirstOrDefault(c => c.Type == "userid")?.Value;

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
            new("userid", request.UserId.ToString())
        };
    }
}