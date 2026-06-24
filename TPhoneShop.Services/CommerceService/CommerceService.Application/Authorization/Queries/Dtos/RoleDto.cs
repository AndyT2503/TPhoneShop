namespace CommerceService.Application.Authorization.Queries.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
