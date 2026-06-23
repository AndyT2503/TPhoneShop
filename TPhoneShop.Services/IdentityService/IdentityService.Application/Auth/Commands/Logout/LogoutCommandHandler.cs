using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.Logout
{
    internal class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IAuthService _authService;
        public LogoutCommandHandler(IdentityDbContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (request.RefreshToken is null)
            {
                return;
            }
            var refreshToken = await _dbContext.RefreshTokens
                                        .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);
            if (refreshToken is null)
            {
                return;
            }
            refreshToken.RevokedAt = DateTimeOffset.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _authService.AddUserSecurityLogAsync(refreshToken.UserId, UserSecurityActions.Logout);
        }
    }
}
