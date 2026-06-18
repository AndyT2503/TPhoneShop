namespace CommerceService.Persistence.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles", CommerceDbContext.AuthSchema);
            builder.HasIndex(e => e.UserId).IsUnique();
            builder.HasIndex(e => e.RoleId);
        }
    }
}
