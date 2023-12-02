using IdentityCore.Model.Audit;
using IdentityCore.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace IdentityInfrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public async Task<bool> CanConnectAsync(CancellationToken cancellationToken)
    {
        return await Database.CanConnectAsync(cancellationToken);
    }

    public virtual DbSet<EndUser> Users { get; set; } = null!;
    public virtual DbSet<AuditRecord> AuditRecords { get; set; } = null!;
}