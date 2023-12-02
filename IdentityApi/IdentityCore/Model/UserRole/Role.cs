using IdentityCore.Model.Users;

namespace IdentityCore.Model.UserRole;

public class Role
{
    public Guid RoleId { get; set; }
    public Guid EndUserId { get; set; }
    public virtual ICollection<EndUser> Users { get; set; }
}