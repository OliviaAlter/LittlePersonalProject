using IdentityCore.Model.UserRole;

namespace IdentityCore.Model.Users;

public class EndUser
{
    public Guid EndUserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] Salt { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public Guid RolesId { get; set; }
    public virtual Roles Roles { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
    public bool IsVerified { get; set; }
    public bool IsLocked { get; set; }
    public DateTime? LockedAt { get; set; }

    public string? Note { get; set; }

    // Token
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreatedAt { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}