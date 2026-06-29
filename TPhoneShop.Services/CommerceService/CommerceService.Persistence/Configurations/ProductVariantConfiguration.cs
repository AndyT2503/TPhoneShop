namespace CommerceService.Persistence.Configurations
{
    internal class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("product_variants", CommerceDbContext.CatalogsSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Sku)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ThumbnailUrl)
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasIndex(x => x.Sku)
                .IsUnique();

            builder.Property(x => x.Price)
                .IsRequired();

            builder.Property(x => x.CompareAtPrice);

            builder.Property(x => x.StockQuantity)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Variants)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
