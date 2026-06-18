using IdentityService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;

namespace IdentityService.Infrastructure.Securities
{
    public class RsaKeyProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RSA PrivateKey { get; private set; } = default!;
        public string KeyId { get; private set; } = default!;

        public RsaKeyProvider(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task LoadAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            var key = await db.SigningKeys
                .FirstAsync(x => x.IsActive);

            KeyId = key.Kid;

            PrivateKey = RSA.Create();
            PrivateKey.ImportFromPem(key.PrivateKey);
        }
    }
}
