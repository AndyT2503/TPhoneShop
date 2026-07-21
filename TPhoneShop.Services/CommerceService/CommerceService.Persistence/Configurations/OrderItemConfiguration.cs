namespace CommerceService.Persistence.Configurations
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items", CommerceDbContext.OrdersSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductVariantId)
                .IsRequired();

            builder.Property(x => x.ProductName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Sku)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.OwnsOne(x => x.UnitPrice);

            builder.OwnsOne(x => x.SubTotal);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.OrderId);

            builder.HasIndex(x => x.ProductVariantId);
        }
    }
}