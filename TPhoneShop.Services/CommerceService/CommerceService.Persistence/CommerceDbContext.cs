namespace CommerceService.Persistence
{
    public class CommerceDbContext : DbContext
    {
        public const string AuthSchema = "auth";
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(CommerceDbContext).Assembly);
        }

    }
}
