using IdentityService.Domain.Entities;
using IdentityService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace IdentityService.Infrastructure.BackgroundJobs
{
    public class KeyRotationHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<KeyRotationHostedService> _logger;
        public KeyRotationHostedService(IServiceScopeFactory scopeFactory, ILogger<KeyRotationHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RotateKey();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromDays(30), stoppingToken);
                await RotateKey();
            }
        }

        private async Task RotateKey()
        {
            _logger.LogInformation("🚀 KeyRotationHostedService STARTED at {time}", DateTime.UtcNow);
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();

            var current = await db.SigningKeys
                .FirstOrDefaultAsync(x => x.IsActive);

            if (current is not null)
            {
                current.IsActive = false;
                current.RevokedAt = DateTime.UtcNow;
            }

            var rsa = RSA.Create(2048);
            var privateKey = rsa.ExportRSAPrivateKeyPem();
            var publicKey = rsa.ExportRSAPublicKeyPem();
            var newKey = new SigningKey
            {
                Kid = Guid.NewGuid().ToString("N"),
                PrivateKey = privateKey,
                PublicKey = publicKey,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                ActivatedAt = DateTime.UtcNow
            };

            db.SigningKeys.Add(newKey);
            await db.SaveChangesAsync();

            _logger.LogInformation("✅ Key rotation completed at {time}", DateTime.UtcNow);
        }
    }
}