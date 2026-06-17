namespace IdentityService.Domain.Entities
{
    public class UserSecurityLog : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public required string Action { get; set; }

        public bool IsSuccess { get; set; }

        public string? FailureReason { get; set; }

        public required string IpAddress { get; set; }

        public required string UserAgent { get; set; }

        public required string DeviceName { get; set; }

    }
}
