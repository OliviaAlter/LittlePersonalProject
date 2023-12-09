using System.ComponentModel.DataAnnotations;

namespace IdentityCore.Model.DatabaseEntity.AuditModel;

public class AuditRecord
{
    [Key] public Guid AuditRecordId { get; set; }

    public required string Action { get; set; }
    public required string Details { get; set; }
    public DateTime Timestamp { get; set; }
}