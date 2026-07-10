using BuildingBlocks.Application.Auth;
using BuildingBlocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Infrastructure.Interceptors
{
    public class SoftDeleteInterceptor(ICurrentUser currentUser) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ApplySoftDelete(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplySoftDelete(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplySoftDelete(DbContext? context)
        {
            if (context is null)
                return;

            var entries = context.ChangeTracker
                .Entries<ISoftDeletable>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                entry.State = EntityState.Modified;

                entry.Entity.DeletedAt = DateTimeOffset.UtcNow;
                entry.Entity.DeletedBy = currentUser.Id;
            }
        }
    }
}
