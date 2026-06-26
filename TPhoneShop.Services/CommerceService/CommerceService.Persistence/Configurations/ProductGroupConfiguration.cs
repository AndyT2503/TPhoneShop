namespace CommerceService.Persistence.Configurations
{
    internal sealed class ProductGroupConfiguration : IEntityTypeConfiguration<ProductGroup>
    {
        public void Configure(EntityTypeBuilder<ProductGroup> builder)
        {
            builder.ToTable("product_groups", CommerceDbContext.ProductsSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Slug)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }
}
