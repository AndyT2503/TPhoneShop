using BuildingBlocks.Infrastructure.Extensions;

namespace CommerceService.Persistence
{
    public class CommerceDbContext : DbContext
    {
        public const string AuthSchema = "auth";
        public const string CatalogsSchema = "catalogs";
        public const string EventsSchema = "events";
        public const string OrdersSchema = "orders";
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderDiscount> OrderDiscounts { get; set; }
        public DbSet<OrderLog> OrderLogs { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(CommerceDbContext).Assembly);

            modelBuilder.ApplySoftDeleteQueryFilter();
        }

    }
}
