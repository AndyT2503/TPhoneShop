namespace IdentityService.Infrastructure.Securities.Options
{
    public class JwtOptions
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string PrivateKeyPath { get; set; }
        public required string PublicKeyPath { get; set; }
        public required string KeyId { get; set; }
        public int AccessTokenMinutes { get; set; }
        public int RefreshTokenDays { get; set; }
    }
}
