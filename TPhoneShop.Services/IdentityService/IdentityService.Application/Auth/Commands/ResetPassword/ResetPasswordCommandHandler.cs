using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.ResetPassword
{
    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IAuthService _authService;
        public ResetPasswordCommandHandler(IdentityDbContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var hashedToken = _authService.HashToken(request.ResetPasswordToken);
            var resetToken = await _dbContext.ResetPasswordTokens
                                    .FirstOrDefaultAsync(e => e.Token == hashedToken, cancellationToken);
            if (resetToken is null ||
                resetToken.ExpiredAt < DateTimeOffset.UtcNow ||
                resetToken.IsUsed
            )
            {
                throw new BadRequestException("Liên kết đặt lại mật khẩu không còn hiệu lực. Vui lòng yêu cầu gửi lại mật khẩu.");
            }

            var user = await _dbContext.Users
                        .FirstOrDefaultAsync(x => x.Id == resetToken.UserId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy người dùng.");
            }
            user.PasswordHash = _authService.HashPassword(request.NewPassword);

            resetToken.IsUsed = true;

            var listAvailableRefreshToken = await _dbContext.RefreshTokens.Where(e => e.UserId == user.Id
                                                                                            && e.RevokedAt == null
                                                                                            && e.ExpiresAt > DateTimeOffset.UtcNow)
                                                                                 .ToListAsync(cancellationToken);
            foreach (var refreshToken in listAvailableRefreshToken)
            {
                refreshToken.ExpiresAt = DateTimeOffset.UtcNow;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await _authService.AddUserSecurityLogAsync(user.Id, UserSecurityActions.ResetPassword);
        }
    }
}
