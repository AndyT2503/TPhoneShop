namespace BuildingBlocks.Application.Auth
{
    public interface ICurrentUser
    {
        Guid? Id { get; }
        string? Email { get; }
    }
}
