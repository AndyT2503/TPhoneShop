namespace FileService.Persistence.Configurations
{
    internal class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.ToTable("medias");
            builder.HasIndex(e => e.Key).IsUnique();
            builder.HasIndex(e => e.ReferrenceId);
        }
    }
}
