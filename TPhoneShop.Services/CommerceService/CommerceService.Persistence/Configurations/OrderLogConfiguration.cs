namespace CommerceService.Persistence.Configurations
{
    public class OrderLogConfiguration : IEntityTypeConfiguration<OrderLog>
    {
        public void Configure(EntityTypeBuilder<OrderLog> builder)
        {
            builder.ToTable("order_logs", CommerceDbContext.OrdersSchema);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Action)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.PaymentStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ShippingStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ShippingMethod)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.PerformedBy)
                .IsRequired();
            builder.Property(x => x.PerfomedAt)
                .IsRequired();

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderLogs)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.OrderId);

            builder.HasIndex(x => new { x.OrderId, x.PerfomedAt });

            builder.HasIndex(x => x.Action);
        }
    }
}