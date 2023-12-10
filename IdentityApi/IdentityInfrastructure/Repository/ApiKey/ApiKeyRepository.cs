using System.Security.Cryptography;
using IdentityCore.Model.DatabaseEntity.ApiKeyModel;
using IdentityCore.Model.ObjectResponse;
using IdentityCore.RepositoryException;
using IdentityCore.RepositoryInterface.ApiKey;
using IdentityInfrastructure.Data;
using IdentityInfrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityInfrastructure.Repository.ApiKey;

public class ApiKeyRepository
    (IAuthIdentityDbContext context, ILogger<ApiKeyRepository> logger) : GenericRepository<UserApiKey>(context),
        IApiKeyRepository
{
    private const int KeySize = 32; // Size of the API key in bytes (256 bits)

    public async Task<string> GetApiKeyAsync(Guid accountId)
    {
        if (accountId == Guid.Empty)
            throw new ArgumentException("Invalid user ID.", nameof(accountId));

        var apiKey = await FindApiKeyBasedOnUserAsync(accountId);

        switch (apiKey)
        {
            case null:
                logger.LogInformation("No API key found for user {UserId}", accountId);
                throw new ApiKeyNotFoundException("API key not found.");

            case { IsRevoked: true }:
                logger.LogInformation("API key for user {UserId} is revoked", accountId);
                throw new ApiKeyRevokedException("API key is revoked.");

            case not null when apiKey.ExpiresAt < DateTime.UtcNow:
                logger.LogInformation("API key for user {UserId} has expired", accountId);
                throw new ApiKeyExpiredException("API key has expired.");
        }

        return apiKey.UniqueApiKey;
    }

    public async Task<string> CreateApiKeyAsync(Guid accountId)
    {
        if (accountId == Guid.Empty)
            throw new ArgumentException("Invalid user ID.", nameof(accountId));

        await using var transaction = await context.BeginTransactionAsync();
        try
        {
            var existingApiKey = await FindApiKeyBasedOnUserAsync(accountId);

            if (existingApiKey is null || existingApiKey.IsRevoked || existingApiKey.ExpiresAt < DateTime.UtcNow)
            {
                // Create a new API key if it doesn't exist or is revoked
                var newApiKey = new UserApiKey
                {
                    UserApiKeyId = Guid.NewGuid(),
                    IsRevoked = false,
                    AccountId = accountId
                };

                UpdateApiKey(newApiKey);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                logger.LogInformation("API key created for user {UserId}", accountId);
                return newApiKey.UniqueApiKey;
            }

            // Update the existing API key
            UpdateApiKey(existingApiKey);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogInformation("API key updated for user {UserId}", accountId);
            return existingApiKey.UniqueApiKey;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Error occurred while creating API key for user {UserId}", accountId);
            throw new ApiKeyGenerationException("Error while creating API key", ex);
        }
    }


    public async Task<bool> RevokeApiKeyAsync(Guid accountId)
    {
        if (accountId == Guid.Empty)
            throw new ArgumentException("Invalid user ID.", nameof(accountId));

        try
        {
            var apiKey = await FindApiKeyBasedOnUserAsync(accountId);

            if (apiKey is null)
            {
                logger.LogInformation("No API key found for user {UserId} to revoke", accountId);
                return false;
            }

            apiKey.IsRevoked = true;
            apiKey.ExpiresAt = DateTime.UtcNow;

            context.UserApiKeys.Update(apiKey);
            await context.SaveChangesAsync();

            logger.LogInformation("API key revoked for user {UserId}", accountId);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while revoking API key for user {UserId}", accountId);
            throw new ApiKeyValidationException("Error while revoking API key", ex);
        }
    }

    public async Task<UserApiKeyResponse?> GetUserFromApiKeyAsync(string providedApiKey)
    {
        if (string.IsNullOrEmpty(providedApiKey))
            throw new ArgumentException("API key must be provided.", nameof(providedApiKey));

        var apiKey = await context.UserApiKeys
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.UniqueApiKey == providedApiKey
                                      && (!x.IsRevoked
                                          || x.ExpiresAt > DateTime.UtcNow));

        if (apiKey is not null)
            return new UserApiKeyResponse
            {
                AccountId = apiKey.Account.UserId,
                Email = apiKey.Account.Email,
                Username = apiKey.Account.Username
            };

        return null;
    }

    public async Task<bool> IsApiKeyValidAsync(string apiKeyId)
    {
        if (string.IsNullOrEmpty(apiKeyId))
            throw new ArgumentException("API key must be provided.", nameof(apiKeyId));

        return await context.UserApiKeys
            .AnyAsync(x =>
                x.UniqueApiKey == apiKeyId
                && !x.IsRevoked
                && x.ExpiresAt > DateTime.UtcNow);
    }

    private async Task<UserApiKey?> FindApiKeyBasedOnUserAsync(Guid accountId)
    {
        return await context.UserApiKeys
            .FirstOrDefaultAsync(x
                => x.AccountId == accountId
                   && !x.IsRevoked
                   && x.ExpiresAt > DateTime.UtcNow);
    }

    private void UpdateApiKey(UserApiKey apiKey)
    {
        apiKey.UniqueApiKey = GenerateSecureKey();
        apiKey.CreatedAt = DateTime.UtcNow;
        apiKey.ExpiresAt = DateTime.UtcNow.AddDays(15);
        context.UserApiKeys.Update(apiKey); // This will handle both add and update
    }

    private static string GenerateSecureKey()
    {
        var randomBytes = new byte[KeySize];
        RandomNumberGenerator.Fill(randomBytes);
        return BitConverter.ToString(randomBytes).Replace("-", "").ToLowerInvariant();
    }
}