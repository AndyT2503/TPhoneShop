using CommerceService.Application.Authorization.Queries.Dtos;

namespace CommerceService.Application.Authorization.Queries.GetListPermission
{
    public record GetListPermissionQuery() : IRequest<ListPermissionResponse>;
}
