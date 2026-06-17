using IdentityService.Application.Common.Abstractions;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityService.Infrastructure.Securities
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _context;

        public CurrentUser(IHttpContextAccessor context)
        {
            _context = context;
        }
        public Guid? Id => Guid.TryParse((User?.FindFirstValue(JwtRegisteredClaimNames.Sub)), out var guid) ? guid : null;

        public string? Email => User?.FindFirstValue(JwtRegisteredClaimNames.Email);

        private ClaimsPrincipal? User => _context.HttpContext?.User;
    }
}
