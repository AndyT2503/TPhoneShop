using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Constants;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Securities.Options;
using IdentityService.Persistence;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Infrastructure.Securities
{
    public class AuthService : IAuthService
    {
        private readonly IdentityDbContext _dbContext;
        private readonly JwtOptions _jwt;
        private readonly RsaKeyProvider _keys;
        private readonly IClientInfoService _clientInfoService;
        public AuthService(
            IdentityDbContext dbContext,
            IOptions<JwtOptions> jwtOptions,
            RsaKeyProvider keys,
            IClientInfoService clientInfoService
        )
        {
            _dbContext = dbContext;
            _jwt = jwtOptions.Value;
            _keys = keys;
            _clientInfoService = clientInfoService;
        }
        public async Task<AuthResponse> GenerateLoginSessionAsync(User user, CancellationToken cancellationToken)
        {
            var accessToken = await GenerateAccessTokenAsync(user, cancellationToken);

            var refreshTokenValue = GenerateRefreshToken();
            var expireRefreshTokenTime = DateTimeOffset.UtcNow.AddDays(_jwt.RefreshTokenDays);
            var ipAddress = _clientInfoService.GetIPAddress();
            var userAgent = _clientInfoService.GetUserAgent();
            var deviceName = _clientInfoService.GetDeviceName();


            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = HashToken(refreshTokenValue),
                ExpiresAt = expireRefreshTokenTime,
                DeviceName = deviceName,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync(cancellationToken);


            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                RefreshTokenExpiresTime = expireRefreshTokenTime,
            };
        }

        public string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public bool VerifyPassword(string plainPassword, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashPassword);
        }

        public string HashToken(string token)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }

        public async Task AddUserSecurityLogAsync(Guid userId, string userSecurityAction, string? failureReason = null)
        {
            if (!UserSecurityActions.IsValid(userSecurityAction))
            {
                throw new ArgumentException("Invalid action");
            }
            var ipAddress = _clientInfoService.GetIPAddress();
            var userAgent = _clientInfoService.GetUserAgent();
            var deviceName = _clientInfoService.GetDeviceName();
            var secrurityLog = new UserSecurityLog
            {
                UserId = userId,
                IsSuccess = failureReason != null,
                FailureReason = failureReason,
                Action = userSecurityAction,
                DeviceName = deviceName,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };
            _dbContext.UserSecurityLogs.Add(secrurityLog);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<string> GenerateAccessTokenAsync(User user, CancellationToken cancellationToken)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
            };

            var key = new RsaSecurityKey(_keys.PrivateKey)
            {
                KeyId = _keys.KeyId
            };

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow
                    .AddMinutes(_jwt.AccessTokenMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString("N")
                   + Guid.NewGuid().ToString("N");
        }
    }
}
