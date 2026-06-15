using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IAuthService _authService;
        private readonly MainDbContext _mainDbContext;
        private readonly IClientInfoService _clientInfoService;
        public LoginCommandHandler(IAuthService authService, MainDbContext mainDbContext, IClientInfoService clientInfoService)
        {
            _authService = authService;
            _mainDbContext = mainDbContext;
            _clientInfoService = clientInfoService;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _mainDbContext.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Email == request.Email, cancellationToken);

            if (user is null)
            {
                throw new UnauthorizedException("Email hoặc mật khẩu không hợp lệ!");
            }
            var verifyPassword = _authService.VerifyPassword(request.Password, user.PasswordHash);
            if (!verifyPassword)
            {

                await RecordFailedLoginAsync(user);
                throw new UnauthorizedException("Email hoặc mật khẩu không hợp lệ!");
            }

            var response = await _authService.GenerateLoginSessionAsync(user, cancellationToken);
            return response;
        }

        private async Task RecordFailedLoginAsync(User user)
        {
            var ipAddress = _clientInfoService.GetIPAddress();
            var userAgent = _clientInfoService.GetUserAgent();
            var deviceName = _clientInfoService.GetDeviceName();
            var loginLog = new UserLoginLog
            {
                UserId = user.Id,
                IsSuccess = false,
                FailureReason = "Mật khẩu không hợp lệ",
                LoginAt = DateTimeOffset.UtcNow,
                DeviceName = deviceName,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };
        }
    }
}
