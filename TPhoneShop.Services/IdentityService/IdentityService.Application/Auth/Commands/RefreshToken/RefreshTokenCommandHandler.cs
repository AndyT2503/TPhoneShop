using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IAuthService _authService;
        private readonly IdentityDbContext _dbContext;

        public RefreshTokenCommandHandler(
            IAuthService authService,
            IdentityDbContext dbContext
        )
        {
            _authService = authService;
            _dbContext = dbContext;
        }

        public async Task<AuthResponse> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(request.RefreshToken))
            {
                throw new UnauthorizedException("Refresh token không hợp lệ!");
            }
            var hashedToken = _authService.HashToken(request.RefreshToken);
            var refreshToken = await _dbContext.RefreshTokens
                                                        .Include(x => x.User)
                                                        .FirstOrDefaultAsync(
                                                            x => x.Token == hashedToken, cancellationToken);

            if (refreshToken is null)
            {
                throw new UnauthorizedException("Refresh token không hợp lệ!");
            }

            if (refreshToken.RevokedAt.HasValue)
            {
                throw new UnauthorizedException("Refresh token đã bị thu hồi!");
            }

            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Refresh token đã hết hạn!");
            }

            refreshToken.RevokedAt = DateTimeOffset.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _authService.AddUserSecurityLogAsync(refreshToken.UserId, UserSecurityActions.RefreshToken);
            var response = await _authService.GenerateLoginSessionAsync(refreshToken.User, cancellationToken);
            return response;
        }
    }
}
