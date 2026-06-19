namespace CommerceService.Application.Authorization.Dtos
{
    public class ListPermissionResponse
    {
        public required List<PermissionDto> Permissions { get; set; }
    }
}
