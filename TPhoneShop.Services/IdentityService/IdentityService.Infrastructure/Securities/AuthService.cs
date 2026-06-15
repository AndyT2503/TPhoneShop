using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Securities.Constants;
using IdentityService.Infrastructure.Securities.Options;
using IdentityService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityService.Infrastructure.Securities
{
    public class AuthService : IAuthService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly JwtOptions _jwt;
        private readonly RsaKeyProvider _keys;
        private readonly IClientInfoService _clientInfoService;
        public AuthService(
            MainDbContext mainDbContext,
            IOptions<JwtOptions> jwtOptions,
            RsaKeyProvider keys,
            IClientInfoService clientInfoService
        )
        {
            _mainDbContext = mainDbContext;
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
                Token = refreshTokenValue,
                ExpiresAt = expireRefreshTokenTime,
                DeviceName = deviceName,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            var loginLog = new UserLoginLog
            {
                UserId = user.Id,
                IsSuccess = true,
                LoginAt = DateTimeOffset.UtcNow,
                DeviceName = deviceName,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            _mainDbContext.RefreshTokens.Add(refreshToken);
            _mainDbContext.UserLoginLogs.Add(loginLog);
            await _mainDbContext.SaveChangesAsync(cancellationToken);

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

        private async Task<string> GenerateAccessTokenAsync(User user, CancellationToken cancellationToken)
        {
            var listPermission = await _mainDbContext.RolePermissions.AsNoTracking()
                                                                     .Where(e => e.RoleId == user.RoleId)
                                                                     .Select(e => e.Permission.Name)
                                                                     .ToListAsync(cancellationToken);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
            };

            foreach (var permission in listPermission)
            {
                claims.Add(
                    new Claim(CustomClaimNames.Permission, permission));
            }

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
