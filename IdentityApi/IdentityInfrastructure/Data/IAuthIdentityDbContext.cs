using IdentityCore.Model.DatabaseEntity.AccountModel;
using IdentityCore.Model.DatabaseEntity.ApiKeyModel;
using IdentityCore.Model.DatabaseEntity.AuditModel;
using IdentityCore.Model.DatabaseEntity.RoleModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace IdentityInfrastructure.Data;

public interface IAuthIdentityDbContext
{
    // DbSet 
    DbSet<Account> Accounts { get; set; }
    DbSet<UserApiKey> UserApiKeys { get; set; }
    DbSet<AccountRole> Roles { get; set; }
    DbSet<AuditRecord> AuditRecords { get; set; }

    // DbContext
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> IsDatabaseAvailableAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction transaction);
    Task RollbackTransactionAsync(IDbContextTransaction transaction);
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}