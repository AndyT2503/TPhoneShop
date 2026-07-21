namespace CommerceService.Persistence.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders", CommerceDbContext.OrdersSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CustomerId)
                .IsRequired();

            builder.Property(x => x.OrderNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.OrderNumber)
                .IsUnique();

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PaymentMethod)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.PaymentStatus)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ShippingMethod)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ShippingStatus)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CustomerNote)
                .HasMaxLength(500);

            builder.Property(x => x.CancelReason)
                .HasMaxLength(500);

            builder.OwnsOne(x => x.SubTotal);

            builder.OwnsOne(x => x.TotalDiscount);

            builder.OwnsOne(x => x.ShippingFee);

            builder.OwnsOne(x => x.Tax);

            builder.OwnsOne(x => x.TotalAmount);

            builder.OwnsOne(x => x.ShippingAddress);

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.OrderDiscounts)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.CustomerId);

            builder.HasIndex(x => x.Status);

            builder.HasIndex(x => x.PaymentStatus);

            builder.HasIndex(x => x.CreatedAt);

            builder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}