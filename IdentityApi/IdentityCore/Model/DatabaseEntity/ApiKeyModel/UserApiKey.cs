using System.ComponentModel.DataAnnotations;
using IdentityCore.Model.DatabaseEntity.AccountModel;

namespace IdentityCore.Model.DatabaseEntity.ApiKeyModel;

public class UserApiKey
{
    [Key] public Guid UserApiKeyId { get; set; }

    public string UniqueApiKey { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
}