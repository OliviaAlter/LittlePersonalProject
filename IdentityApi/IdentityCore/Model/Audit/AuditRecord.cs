namespace IdentityCore.Model.Audit;

public class AuditRecord
{
    public int Id { get; set; }
    public required string Action { get; set; }
    public required string Details { get; set; }
    public DateTime Timestamp { get; set; }
}