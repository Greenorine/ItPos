using ItPos.Domain.Interfaces;
using ItPos.Domain.Models;
using ItPos.Domain.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess;

public sealed class ItPosDbContext : DbContext
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public ItPosDbContext(DbContextOptions<ItPosDbContext> options, IHttpContextAccessor httpContextAccessor) :
        base(options)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        WriteAudit();
        WriteTime();
        SoftDelete();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<PosUser>().HasAlternateKey(x => x.Login);
        base.OnModelCreating(builder);
    }

    private void SoftDelete()
    {
        foreach (var entry in ChangeTracker.Entries<IHistoricalEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    entry.Entity.IsDeleted = true;
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Modified:
                    break;
                case EntityState.Added:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void WriteTime()
    {
        foreach (var entry in ChangeTracker.Entries<ITimedEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    break;
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void WriteAudit()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedBy = httpContextAccessor?.HttpContext?.User.Identity?.Name ?? "Admin";
                    break;
                case EntityState.Added:
                    entry.Entity.ModifiedBy = httpContextAccessor?.HttpContext?.User.Identity?.Name ?? "Admin";
                    entry.Entity.CreatedBy = httpContextAccessor?.HttpContext?.User.Identity?.Name ?? "Admin";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public DbSet<StudentInfo> Students { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<PosUser> Users { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<Application> Applications { get; set; }
}