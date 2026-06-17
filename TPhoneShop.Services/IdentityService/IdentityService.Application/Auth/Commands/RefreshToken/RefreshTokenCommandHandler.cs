using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Services;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IAuthService _authService;
        private readonly MainDbContext _mainDbContext;

        public RefreshTokenCommandHandler(
            IAuthService authService,
            MainDbContext mainDbContext
        )
        {
            _authService = authService;
            _mainDbContext = mainDbContext;
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
            var refreshToken = await _mainDbContext.RefreshTokens
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
            await _mainDbContext.SaveChangesAsync(cancellationToken);

            var response = await _authService.GenerateLoginSessionAsync(refreshToken.User, cancellationToken);
            return response;
        }
    }
}
