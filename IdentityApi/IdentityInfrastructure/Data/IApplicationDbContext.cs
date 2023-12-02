using IdentityCore.Model.Audit;
using IdentityCore.Model.UserRole;
using IdentityCore.Model.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IdentityInfrastructure.Data;

public interface IApplicationDbContext
{
    // DbSet 
    DbSet<EndUser> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<AuditRecord> AuditRecords { get; set; }

    // DbContext
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> CanConnectAsync(CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}