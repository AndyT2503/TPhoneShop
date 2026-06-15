using IdentityService.Application.Auth.Services;
using IdentityService.Infrastructure.Securities.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdentityService.API.Controllers
{
    [ApiController]
    public class DiscoveryController : ControllerBase
    {
        private readonly IJwksService _jwksService;

        private readonly JwtOptions _jwt;

        public DiscoveryController(
            IJwksService jwksService,
            IOptions<JwtOptions> jwt)
        {
            _jwksService = jwksService;

            _jwt = jwt.Value;
        }

        [HttpGet("/.well-known/openid-configuration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult OpenIdConfiguration()
        {
            var issuer = _jwt.Issuer.TrimEnd('/');

            return Ok(new
            {
                issuer,

                jwks_uri =
                    $"{issuer}/.well-known/jwks.json"
            });
        }

        [HttpGet("/.well-known/jwks.json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Jwks()
        {
            return Ok(await _jwksService.GetJwks());
        }
    }
}
