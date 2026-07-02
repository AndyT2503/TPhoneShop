namespace FileService.Persistence
{
    public class FileDbContext : DbContext
    {
        public DbSet<Domain.Entities.Media> Medias { get; set; }


        public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileDbContext).Assembly);
        }

    }
}
