using IdentityCore.Model.Users;

namespace IdentityCore.Model.UserRole;

public class Roles
{
    public Roles()
    {
        Users = new HashSet<EndUser>();
    }

    public Guid RolesId { get; set; }
    public string RoleName { get; set; }
    public virtual ICollection<EndUser> Users { get; set; }
}