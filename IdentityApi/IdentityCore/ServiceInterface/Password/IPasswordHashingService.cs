namespace IdentityCore.ServiceInterface.Password;

public interface IPasswordHashingService
{
    Task<(byte[] Hash, byte[] Salt)> CreatePasswordHashAsync(string password);
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}