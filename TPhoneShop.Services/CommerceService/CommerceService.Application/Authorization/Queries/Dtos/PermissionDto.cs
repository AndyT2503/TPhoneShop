namespace CommerceService.Application.Authorization.Queries.Dtos
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
