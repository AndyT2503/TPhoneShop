namespace IdentityService.Domain.Entities
{
    public class SigningKey : BaseEntity
    {
        public required string Kid { get; set; }

        public required string PrivateKey { get; set; }

        public required string PublicKey { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? RevokedAt { get; set; }

        public DateTimeOffset ActivatedAt { get; set; }
    }
}
