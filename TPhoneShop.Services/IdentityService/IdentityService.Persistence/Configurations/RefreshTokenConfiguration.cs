namespace IdentityService.Persistence.Configurations
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Token)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.DeviceName)
                .HasMaxLength(200);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(100);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.Token)
                .IsUnique();
        }
    }
}
