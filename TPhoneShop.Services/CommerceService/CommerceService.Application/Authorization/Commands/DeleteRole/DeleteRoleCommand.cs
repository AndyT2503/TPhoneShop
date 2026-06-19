namespace CommerceService.Application.Authorization.Commands.DeleteRole
{
    public record DeleteRoleCommand(Guid RoleId) : IRequest;
}
