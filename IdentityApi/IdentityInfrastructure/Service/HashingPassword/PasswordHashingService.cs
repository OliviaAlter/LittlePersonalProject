using System.Security.Cryptography;
using System.Text;
using IdentityCore.ServiceInterface.Password;

namespace IdentityInfrastructure.Service.HashingPassword;

public class PasswordHashingService : IPasswordHashingService
{
    private const int MaxPasswordLength = 50;

    public async Task<(byte[] Hash, byte[] Salt)> CreatePasswordHashAsync(string password)
    {
        ArgumentNullException.ThrowIfNull(password);
        if (password.Length > MaxPasswordLength)
            throw new ArgumentException("Password length must not exceed 50 characters.");

        return await Task.Run(() =>
        {
            using var hmac = new HMACSHA512();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (passwordHash, passwordSalt);
        });
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        ArgumentNullException.ThrowIfNull(password);
        if (password.Length > MaxPasswordLength)
            throw new ArgumentException("Password length must not exceed 50 characters.");

        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }
}