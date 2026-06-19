using IdentityService.Application.Auth.Services;
using IdentityService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace IdentityService.Infrastructure.Securities
{
    internal class JwksService : IJwksService
    {
        private readonly IdentityDbContext _dbContext;

        public JwksService(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<object> GetJwks()
        {
            var signingKeys = await _dbContext.SigningKeys
                                    .Where(x => x.RevokedAt == null)
                                    .ToListAsync();

            var jwksKeys = signingKeys.Select(key =>
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(key.PublicKey);

                var jwk = JsonWebKeyConverter
                    .ConvertFromRSASecurityKey(new RsaSecurityKey(rsa));

                jwk.Kid = key.Kid;
                jwk.Use = "sig";
                jwk.Alg = "RS256";

                return jwk;
            });
            return new { keys = jwksKeys };
        }
    }
}
