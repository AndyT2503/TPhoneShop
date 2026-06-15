namespace IdentityService.Persistence.Configurations
{
    public class SigningKeyConfiguration : IEntityTypeConfiguration<SigningKey>
    {
        public void Configure(EntityTypeBuilder<SigningKey> builder)
        {
            builder.ToTable("signing_keys");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Kid)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Kid)
                .IsUnique();

            builder.Property(x => x.PrivateKey)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(x => x.PublicKey)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasIndex(x => x.IsActive);

            builder.Property(x => x.ActivatedAt);

            builder.Property(x => x.RevokedAt);

            builder.HasIndex(x => x.RevokedAt);
        }
    }
}
