namespace CommerceService.Persistence.Configurations
{
    internal class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("coupons", CommerceDbContext.OrdersSchema);

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

            builder.Property(x => x.UsageLimit);

            builder.Property(x => x.PerUserUsageLimit);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.OwnsOne(x => x.MinimumOrderAmount);

            builder.OwnsOne(x => x.MaximumDiscountAmount);

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.HasIndex(x => x.IsActive);

            builder.HasIndex(x => new
            {
                x.IsActive,
                x.StartsAt,
                x.ExpiresAt
            });

            builder.HasQueryFilter(x => x.DeletedAt == null);
        }
    }
}