using BuildingBlocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuildingBlocks.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplySoftDeleteQueryFilter(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                    continue;

                var parameter = Expression.Parameter(entityType.ClrType, "e");

                var deletedAt = Expression.Property(
                    parameter,
                    nameof(ISoftDeletable.DeletedAt));

                var body = Expression.Equal(
                    deletedAt,
                    Expression.Constant(null));

                var lambda = Expression.Lambda(body, parameter);

                modelBuilder
                    .Entity(entityType.ClrType)
                    .HasQueryFilter(lambda);
            }

            return modelBuilder;
        }
    }
}
