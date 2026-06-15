namespace IdentityService.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public required string Token { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset? RevokedAt { get; set; }
        public required string DeviceName { get; set; }
        public required string IpAddress { get; set; }
        public required string UserAgent { get; set; }
    }
}
