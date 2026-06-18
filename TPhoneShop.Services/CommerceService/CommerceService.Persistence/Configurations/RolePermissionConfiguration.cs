namespace CommerceService.Persistence.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("role_permissions", CommerceDbContext.AuthSchema);

            builder.HasKey(x => new
            {
                x.RoleId,
                x.PermissionId
            });
        }
    }
}
