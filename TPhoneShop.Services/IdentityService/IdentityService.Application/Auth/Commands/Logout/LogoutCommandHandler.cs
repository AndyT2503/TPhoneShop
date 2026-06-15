using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly MainDbContext _mainDbContext;
        public LogoutCommandHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
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
        }
    }
}
