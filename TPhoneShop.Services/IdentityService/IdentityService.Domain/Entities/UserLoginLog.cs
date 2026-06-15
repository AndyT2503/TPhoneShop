namespace IdentityService.Domain.Entities
{
    public class UserLoginLog : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public bool IsSuccess { get; set; }

        public string? FailureReason { get; set; }

        public required string IpAddress { get; set; }

        public required string UserAgent { get; set; }

        public required string DeviceName { get; set; }

        public DateTimeOffset LoginAt { get; set; }

    }
}
