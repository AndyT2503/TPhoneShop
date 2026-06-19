namespace IdentityService.Persistence.Configurations
{
    internal class ResetPasswordTokenConfiguration : IEntityTypeConfiguration<ResetPasswordToken>
    {
        public void Configure(EntityTypeBuilder<ResetPasswordToken> builder)
        {
            builder.ToTable("reset_password_tokens");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(x => x.Token)
                .IsUnique();

            builder.Property(x => x.ExpiredAt)
                .IsRequired();

            builder.Property(x => x.IsUsed)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
