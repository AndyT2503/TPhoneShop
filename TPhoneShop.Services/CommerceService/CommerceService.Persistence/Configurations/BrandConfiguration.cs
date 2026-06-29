namespace CommerceService.Persistence.Configurations
{
    internal class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("brands", CommerceDbContext.CatalogsSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Slug)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LogoUrl)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasIndex(x => x.Slug)
                .IsUnique();
        }
    }
}
