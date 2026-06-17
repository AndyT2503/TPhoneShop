namespace IdentityService.Application.Common.Abstractions
{
    public interface ICurrentUser
    {
        Guid? Id { get; }
        string? Email { get; }
    }
}
