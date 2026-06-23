namespace IdentityService.Persistence.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.PasswordHash);

            builder.HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}
