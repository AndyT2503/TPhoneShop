namespace CommerceService.Application.Authorization.Dtos
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
