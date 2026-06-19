using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Securities;
using IdentityService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace IdentityService.Infrastructure.BackgroundJobs
{
    internal class KeyRotationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RsaKeyProvider _rsaKeyProvider;
        private readonly ILogger<KeyRotationService> _logger;
        private static readonly TimeSpan KeyLifetime = TimeSpan.FromDays(30);
        public KeyRotationService(IServiceScopeFactory scopeFactory, ILogger<KeyRotationService> logger, RsaKeyProvider rsaKeyProvider)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _rsaKeyProvider = rsaKeyProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await EnsureActiveKey();
                await _rsaKeyProvider.LoadAsync();
                await Task.Delay(TimeSpan.FromDays(30), stoppingToken);
            }
        }

        private async Task EnsureActiveKey()
        {
            _logger.LogInformation("Checking signing key status...");

            await using var scope = _scopeFactory.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            var activeKey = await db.SigningKeys
                .FirstOrDefaultAsync(x => x.IsActive);

            if (activeKey is null)
            {
                _logger.LogInformation("No active signing key found. Creating initial key.");
                await CreateNewKeyAsync(db);
                return;
            }

            var expiresAt = activeKey.ActivatedAt.Add(KeyLifetime);

            if (expiresAt > DateTimeOffset.UtcNow)
            {
                _logger.LogInformation("Active signing key is still valid until {ExpiresAt}. Rotation skipped.", expiresAt);
                return;
            }

            _logger.LogInformation("Active signing key expired at {ExpiresAt}. Rotating key.", expiresAt);

            activeKey.IsActive = false;
            activeKey.RevokedAt = DateTimeOffset.UtcNow;

            await CreateNewKeyAsync(db);
            _logger.LogInformation("Signing key rotated successfully.");
        }

        private static async Task CreateNewKeyAsync(IdentityDbContext db)
        {
            using var rsa = RSA.Create(2048);

            var signingKey = new SigningKey
            {
                Kid = Guid.NewGuid().ToString("N"),
                PrivateKey = rsa.ExportRSAPrivateKeyPem(),
                PublicKey = rsa.ExportRSAPublicKeyPem(),
                IsActive = true,
                ActivatedAt = DateTime.UtcNow
            };
            db.SigningKeys.Add(signingKey);

            await db.SaveChangesAsync();
        }
    }
}