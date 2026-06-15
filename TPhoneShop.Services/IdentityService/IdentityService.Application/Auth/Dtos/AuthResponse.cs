namespace IdentityService.Application.Auth.Dtos
{
    public class AuthResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpiresTime { get; set; }
    }
}
