using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.Login
{
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IAuthService _authService;
        private readonly IdentityDbContext _dbContext;
        public LoginCommandHandler(IAuthService authService, IdentityDbContext dbContext)
        {
            _authService = authService;
            _dbContext = dbContext;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Email == request.Email, cancellationToken);

            if (user is null)
            {
                throw new UnauthorizedException("Email hoặc mật khẩu không hợp lệ.");
            }

            if (user.PasswordHash is null)
            {
                throw new UnauthorizedException("Email hoặc mật khẩu không hợp lệ.");
            }

            var verifyPassword = _authService.VerifyPassword(request.Password, user.PasswordHash);
            if (!verifyPassword)
            {

                await _authService.AddUserSecurityLogAsync(user.Id, UserSecurityActions.Login, "Mật khẩu không hợp lệ");
                throw new UnauthorizedException("Email hoặc mật khẩu không hợp lệ.");
            }

            await _authService.AddUserSecurityLogAsync(user.Id, UserSecurityActions.Login);
            var response = await _authService.GenerateLoginSessionAsync(user, cancellationToken);
            return response;
        }
    }
}
