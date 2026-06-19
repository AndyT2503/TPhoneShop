using CommerceService.Application.Authorization.Dtos;

namespace CommerceService.Application.Authorization.Queries.GetListPermission
{
    public record GetListPermissionQuery() : IRequest<ListPermissionResponse>;
}
