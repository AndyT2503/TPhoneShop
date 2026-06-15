using IdentityService.Application.Auth.Services;
using IdentityService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace IdentityService.Infrastructure.Securities
{
    public class JwksService : IJwksService
    {
        private readonly MainDbContext _mainDbContext;

        public JwksService(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<object> GetJwks()
        {
            var signingKeys = await _mainDbContext.SigningKeys
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
