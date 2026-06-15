namespace IdentityService.Domain.Entities
{
    public class SigningKey : BaseEntity
    {
        public required string Kid { get; set; }

        public required string PrivateKey { get; set; }

        public required string PublicKey { get; set; }

        public bool IsActive { get; set; }

        public DateTime? RevokedAt { get; set; }

        public DateTime? ActivatedAt { get; set; }
    }
}
