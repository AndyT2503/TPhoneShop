namespace IdentityService.Persistence.Configurations
{
    internal class UserSecurityLogConfiguration : IEntityTypeConfiguration<UserSecurityLog>
    {
        public void Configure(EntityTypeBuilder<UserSecurityLog> builder)
        {
            builder.ToTable("user_security_logs");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Action);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.UserAgent)
                .HasMaxLength(1000);

            builder.Property(x => x.DeviceName)
                .HasMaxLength(200);

            builder.Property(x => x.FailureReason)
                .HasMaxLength(500);
        }
    }
}
