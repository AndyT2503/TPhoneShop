namespace BuildingBlocks.Domain.Entities
{
    public abstract class BaseEntity : IAuditableEntity
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
