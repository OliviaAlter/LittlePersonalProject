using IdentityCore.Model.DatabaseEntity.Users;

namespace IdentityCore.Model.DatabaseEntity.ApiKey;

public class UserApiKey
{
    public Guid UserApiKeyId { get; set; }
    public string UniqueApiKey { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public Guid EndUserId { get; set; }
    public virtual EndUser User { get; set; }
}