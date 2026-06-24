using CommerceService.Application.Authorization.Queries.Dtos;

namespace CommerceService.Application.Authorization.Queries.GetPermissionsByRole
{
    public record GetPermissionsByRoleQuery(Guid RoleId) : IRequest<ListPermissionResponse>;
}
