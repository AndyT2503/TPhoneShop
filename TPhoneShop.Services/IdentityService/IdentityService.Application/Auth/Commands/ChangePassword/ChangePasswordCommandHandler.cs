using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Services;
using IdentityService.Application.Common.Abstractions;
using IdentityService.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.ChangePassword
{
    internal class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, AuthResponse>
    {
        private readonly ICurrentUser _currentUser;
        private readonly MainDbContext _mainDbContext;
        private readonly IAuthService _authService;
        public ChangePasswordCommandHandler(ICurrentUser currentUser, MainDbContext mainDbContext, IAuthService authService)
        {
            _currentUser = currentUser;
            _mainDbContext = mainDbContext;
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(e => e.Id == _currentUser.Id, cancellationToken);
            if (user == null)
            {
                throw new UnauthorizedException("Phiên đăng nhập không hợp lệ");
            }
            var verifyOldPassword = _authService.VerifyPassword(request.OldPassword, user.PasswordHash);
            if (!verifyOldPassword)
            {
                await _authService.AddUserSecurityLogAsync(user.Id, UserSecurityActions.ChangePassword, "Sai mật khẩu cũ");
                throw new BadRequestException("Mật khẩu cũ không đúng");
            }
            user.PasswordHash = _authService.HashPassword(request.NewPassword);
            var listAvailableRefreshToken = await _mainDbContext.RefreshTokens.Where(e => e.UserId == _currentUser.Id
                                                                                && e.RevokedAt == null
                                                                                && e.ExpiresAt > DateTimeOffset.UtcNow)
                                                                     .ToListAsync(cancellationToken);
            foreach (var refreshToken in listAvailableRefreshToken)
            {
                refreshToken.ExpiresAt = DateTimeOffset.UtcNow;
            }
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            await _authService.AddUserSecurityLogAsync(user.Id, UserSecurityActions.ChangePassword);

            var response = await _authService.GenerateLoginSessionAsync(user, cancellationToken);
            return response;
        }
    }
}

