namespace IdentityService.Persistence.Configurations
{
    public class UserLoginLogConfiguration
    : IEntityTypeConfiguration<UserLoginLog>
    {
        public void Configure(EntityTypeBuilder<UserLoginLog> builder)
        {
            builder.ToTable("user_login_logs");

            builder.HasKey(x => x.Id);

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
