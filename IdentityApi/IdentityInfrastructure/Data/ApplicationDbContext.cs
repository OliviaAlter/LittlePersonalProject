using IdentityCore.Model.Audit;
using IdentityCore.Model.UserRole;
using IdentityCore.Model.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace IdentityInfrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public async Task<bool> IsDatabaseAvailableAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Check if the database can be connected to
            var canConnect = await Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
                return false;

            // Check if the database exists
            var databaseExists = await Database.GetService<IRelationalDatabaseCreator>()
                .ExistsAsync(cancellationToken);

            return databaseExists;
        }
        catch
        {
            // Handle or log the exception as needed
            return false;
        }
    }


    public virtual DbSet<EndUser> Users { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<AuditRecord> AuditRecords { get; set; } = null!;
}