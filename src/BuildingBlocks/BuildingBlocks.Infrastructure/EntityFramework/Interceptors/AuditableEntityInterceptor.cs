using BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Infrastructure.EntityFramework.Interceptors;

public sealed class AuditableEntityInterceptor(
    TimeProvider timeProvider
) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        foreach (EntityEntry<IAuditableEntity> entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified) &&
                !entry.References.Any(r =>
                    r.TargetEntry is not null &&
                    r.TargetEntry.Metadata.IsOwned() &&
                    r.TargetEntry.State is EntityState.Added or EntityState.Modified))
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAtUtc = timeProvider.GetUtcNow().UtcDateTime;
            }

            entry.Entity.UpdatedAtUtc = timeProvider.GetUtcNow().UtcDateTime;
        }
    }
}
