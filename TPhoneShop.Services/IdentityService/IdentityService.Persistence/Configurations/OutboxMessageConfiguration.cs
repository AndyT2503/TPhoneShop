namespace IdentityService.Persistence.Configurations
{
    public class OutboxMessageConfiguration
    : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("outbox_messages");

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
