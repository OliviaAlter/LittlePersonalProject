using IdentityCore.Model.DatabaseEntity.ApiKey;
using IdentityCore.Model.DatabaseEntity.Audit;
using IdentityCore.Model.DatabaseEntity.UserRole;
using IdentityCore.Model.DatabaseEntity.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace IdentityInfrastructure.Data;

public interface IApplicationDbContext
{
    // DbSet 
    DbSet<EndUser> Users { get; set; }
    DbSet<UserApiKey> UserApiKeys { get; set; }
    DbSet<Roles> Roles { get; set; }
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