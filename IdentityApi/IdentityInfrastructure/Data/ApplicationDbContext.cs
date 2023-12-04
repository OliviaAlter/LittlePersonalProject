using IdentityCore.Model.DatabaseEntity.ApiKey;
using IdentityCore.Model.DatabaseEntity.Audit;
using IdentityCore.Model.DatabaseEntity.UserRole;
using IdentityCore.Model.DatabaseEntity.Users;
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

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
    }


    public virtual DbSet<EndUser> Users { get; set; } = null!;
    public virtual DbSet<UserApiKey> UserApiKeys { get; set; }
    public virtual DbSet<Roles> Roles { get; set; } = null!;
    public virtual DbSet<AuditRecord> AuditRecords { get; set; } = null!;
}