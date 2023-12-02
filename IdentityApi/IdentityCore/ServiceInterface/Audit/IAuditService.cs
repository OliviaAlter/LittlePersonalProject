namespace IdentityCore.ServiceInterface.Audit;

public interface IAuditService
{
    Task LogToDatabaseAsync(string action, string details);
}