using IdentityCore.Model.DatabaseEntity.Audit;
using IdentityCore.ServiceInterface.Audit;
using IdentityInfrastructure.Data;

namespace IdentityInfrastructure.Service.Audit;

public class AuditService(IApplicationDbContext dbContext) : IAuditService
{
    public async Task LogToDatabaseAsync(string action, string details)
    {
        var auditRecord = new AuditRecord
        {
            Action = action,
            Details = details,
            Timestamp = DateTime.UtcNow
        };
        dbContext.AuditRecords.Add(auditRecord);
        await dbContext.SaveChangesAsync();
    }
}