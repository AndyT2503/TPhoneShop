namespace IdentityService.Application.Auth.Queries.Dtos
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string FullName { get; set; }
    }
}
