using System.Text.Json;

namespace CommerceService.Persistence.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products", CommerceDbContext.CatalogsSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Slug)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Description);

            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Attributes)
                            .HasColumnType("jsonb")
                            .HasConversion(
                                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                                v => JsonSerializer.Deserialize<List<ProductAttribute>>(v, (JsonSerializerOptions?)null)
                                    ?? new List<ProductAttribute>()
                            );

            builder.HasOne(x => x.ProductGroup)
                        .WithMany(x => x.Products)
                        .HasForeignKey(x => x.ProductGroupId)
                        .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
