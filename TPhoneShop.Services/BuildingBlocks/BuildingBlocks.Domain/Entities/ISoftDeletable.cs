namespace BuildingBlocks.Domain.Entities
{
    public interface ISoftDeletable
    {
        DateTimeOffset? DeletedAt { get; set; }
        Guid? DeletedBy { get; set; }
    }
}
