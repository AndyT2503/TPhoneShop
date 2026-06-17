using IdentityService.Application.Auth.Services;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IAuthService _authService;
        public ResetPasswordCommandHandler(MainDbContext mainDbContext, IAuthService authService)
        {
            _mainDbContext = mainDbContext;
            _authService = authService;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var hashedToken = _authService.HashToken(request.ResetPasswordToken);
            var resetToken = await _mainDbContext.ResetPasswordTokens
                                    .FirstOrDefaultAsync(e => e.Token == hashedToken, cancellationToken);
            if (resetToken is null ||
                resetToken.ExpiredAt < DateTimeOffset.UtcNow ||
                resetToken.IsUsed
            )
            {
                throw new BadRequestException("Liên kết đặt lại mật khẩu không còn hiệu lực. Vui lòng yêu cầu gửi lại mật khẩu.");
            }

            var user = await _mainDbContext.Users
                        .FirstOrDefaultAsync(x => x.Id == resetToken.UserId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy người dùng.");
            }
            user.PasswordHash = _authService.HashPassword(request.NewPassword);

            resetToken.IsUsed = true;

            await _mainDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
