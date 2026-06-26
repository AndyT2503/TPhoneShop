using BuildingBlocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Infrastructure.Configurations
{
    public abstract class OutboxMessageConfigurationBase<TOutboxMessage> : IEntityTypeConfiguration<TOutboxMessage> where TOutboxMessage : OutboxMessageBase
    {
        private readonly string _tableName;
        private readonly string? _schema;

        protected OutboxMessageConfigurationBase(
            string tableName = "outbox_messages",
            string? schema = null)
        {
            _tableName = tableName;
            _schema = schema;
        }

        public virtual void Configure(EntityTypeBuilder<TOutboxMessage> builder)
        {
            builder.ToTable(_tableName, _schema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Payload)
                    .HasColumnType("jsonb")
                    .IsRequired();

            builder.HasIndex(x => new
            {
                x.Status,
                x.CreatedAt
            });

            builder.HasIndex(x => x.ExpiresAt);
        }
    }
}
