using System.ComponentModel.DataAnnotations;
using IdentityCore.Model.DatabaseEntity.AccountModel;

namespace IdentityCore.Model.DatabaseEntity.RoleModel;

public class AccountRole
{
    [Key] public Guid RoleId { get; set; }

    public required string RoleName { get; set; }
    public ICollection<Account> Users { get; set; }
}