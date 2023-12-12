using System.ComponentModel.DataAnnotations;
using IdentityCore.Model.DatabaseEntity.RoleModel;

namespace IdentityCore.Model.DatabaseEntity.AccountModel;

public class Account
{
    [Key] public Guid AccountId { get; set; }

    public required string Username { get; set; }
    public required string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] Salt { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public Guid RolesId { get; set; }
    public AccountRole AccountRole { get; set; } = null!;
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