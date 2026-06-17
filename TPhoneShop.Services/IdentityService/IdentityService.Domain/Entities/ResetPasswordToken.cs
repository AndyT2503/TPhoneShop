namespace IdentityService.Domain.Entities
{
    public class ResetPasswordToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public required string Token { get; set; }
        public DateTimeOffset ExpiredAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
