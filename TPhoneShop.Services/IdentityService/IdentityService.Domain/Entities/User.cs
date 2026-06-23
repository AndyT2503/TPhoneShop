namespace IdentityService.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Email { get; set; }
        public string? PasswordHash { get; set; }
        public required string FullName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
