using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IAuthService _authService;
        public LogoutCommandHandler(MainDbContext mainDbContext, IAuthService authService)
        {
            _mainDbContext = mainDbContext;
            _authService = authService;
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (request.RefreshToken is null)
            {
                return;
            }
            var refreshToken = await _mainDbContext.RefreshTokens
                                        .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);
            if (refreshToken is null)
            {
                return;
            }
            refreshToken.RevokedAt = DateTimeOffset.UtcNow;
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            await _authService.AddUserSecurityLogAsync(refreshToken.UserId, UserSecurityActions.Logout);
        }
    }
}
