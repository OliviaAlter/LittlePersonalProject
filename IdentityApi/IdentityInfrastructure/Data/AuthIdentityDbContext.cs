using IdentityCore.Model.DatabaseEntity.AccountModel;
using IdentityCore.Model.DatabaseEntity.ApiKeyModel;
using IdentityCore.Model.DatabaseEntity.AuditModel;
using IdentityCore.Model.DatabaseEntity.RoleModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace IdentityInfrastructure.Data;

public class AuthIdentityDbContext : DbContext, IAuthIdentityDbContext
{
    public AuthIdentityDbContext(DbContextOptions<AuthIdentityDbContext> options) : base(options)
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


    public DbSet<Account> Accounts { get; set; }
    public DbSet<UserApiKey> UserApiKeys { get; set; }
    public DbSet<AccountRole> Roles { get; set; }
    public DbSet<AuditRecord> AuditRecords { get; set; }
}