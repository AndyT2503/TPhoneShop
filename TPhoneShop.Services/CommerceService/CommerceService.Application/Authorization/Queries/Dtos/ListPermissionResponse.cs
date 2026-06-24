namespace CommerceService.Application.Authorization.Queries.Dtos
{
    public class ListPermissionResponse
    {
        public required List<PermissionDto> Permissions { get; set; }
    }
}
