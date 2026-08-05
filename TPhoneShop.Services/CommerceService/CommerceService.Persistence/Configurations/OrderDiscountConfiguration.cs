namespace CommerceService.Persistence.Configurations
{
    internal class OrderDiscountConfiguration : IEntityTypeConfiguration<OrderDiscount>
    {
        public void Configure(EntityTypeBuilder<OrderDiscount> builder)
        {
            builder.ToTable("order_discounts", CommerceDbContext.OrdersSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.DiscountType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.DiscountValue)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.OwnsOne(x => x.AppliedAmount);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderDiscounts)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Coupon)
                .WithMany()
                .HasForeignKey(x => x.CouponId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.OrderId);

            builder.HasIndex(x => x.CouponId);

            builder.HasIndex(x => x.Code);
        }
    }
}