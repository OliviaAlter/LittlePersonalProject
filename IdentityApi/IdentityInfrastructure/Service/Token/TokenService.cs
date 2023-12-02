using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityCore.Model.Token;
using IdentityCore.Model.Users;
using IdentityCore.RepositoryInterface.User;
using IdentityCore.ServiceInterface.Token;
using IdentityInfrastructure.Setting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityInfrastructure.Service.Token;

public class TokenService
    (JwtSettings jwtSettings, IEndUserRepository repository) : ITokenService
{
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
            return string.Empty;
        }
    }

    public async Task<(string jwtToken, string refreshToken)> GenerateTokenForUser(EndUser user)
    {
        var claims = GenerateClaimsForUser(user);

        var jwtToken = CreateJwtToken(claims); // Implement this method to generate JWT

        var refreshToken = GenerateRefreshToken(); // Implement this method to generate a refresh token

        // Save refresh token with user in the database
        user.RefreshToken = refreshToken;

        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set expiry, e.g., 7 days

        await repository.UpdatePartialAsync(user);

        return (jwtToken, refreshToken);
    }

    public async Task<(string jwtToken, string refreshToken)> RefreshTokenForUser(TokenGeneration request)
    {
        var user = await GetUserFromToken(request.Token);

        // Validate the refresh token
        if (!await IsRefreshTokenValid(request.RefreshToken, user.EndUserId))
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

    public async Task<bool> IsRefreshTokenValid(string refreshToken, Guid userId)
    {
        var localUserId = userId;

        var user = await repository.FindAsync(x => x.EndUserId == localUserId);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new InvalidOperationException("Invalid client request");

        return user.RefreshToken == refreshToken && user.RefreshTokenExpiryTime > DateTime.UtcNow;
    }

    private async Task<EndUser> GetUserFromToken(string token)
    {
        var userIdClaim = await GetUserIdFromClaims(token);

        if (!Guid.TryParse(userIdClaim, out var userId))
            throw new InvalidOperationException("Invalid token");

        var userToFind = userId;

        var user = await repository.FindAsync(x => x.EndUserId == userToFind);

        if (user is null)
            throw new InvalidOperationException("Invalid token");

        return user;
    }

    public async Task<Guid> GetUserIdFromToken(string token)
    {
        var userIdClaim = await GetUserIdFromClaims(token);
        
        if (Guid.TryParse(userIdClaim, out var userId))
            return await Task.FromResult(userId);

        throw new ArgumentException("Invalid token");
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

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8
                .GetBytes(jwtSettings.NotTokenKeyForSureSourceTrustMeDude
                          ?? throw new InvalidOperationException("Jwt secret is missing")));

        var credential = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(8)),
            Issuer = jwtSettings.Issuer,
            Audience = string.Join(" ", jwtSettings.Audiences),
            SigningCredentials = credential
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
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
        var key = Encoding.UTF8.GetBytes(jwtSettings.NotTokenKeyForSureSourceTrustMeDude)
                  ?? throw new InvalidOperationException("Jwt secret is missing");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
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

    private static List<Claim> GenerateClaimsForUser(EndUser request)
    {
        return new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, request.Email),
            new(JwtRegisteredClaimNames.Email, request.Email),
            new("userid", request.EndUserId.ToString())
        };
    }
}