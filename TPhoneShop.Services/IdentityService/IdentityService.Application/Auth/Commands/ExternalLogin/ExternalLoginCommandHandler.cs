using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Constants;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.ExternalLogin
{
    internal class ExternalLoginCommandHandler : IRequestHandler<ExternalLoginCommand, AuthResponse>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IAuthService _authService;
        public ExternalLoginCommandHandler(IdentityDbContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            ExternalUserInfo externalUserInfo;
            try
            {
                externalUserInfo = await _authService.VerifyExternalUserLoginAsync(request.IdToken, cancellationToken);
            }
            catch (Exception)
            {
                throw new UnauthorizedException("Thông tin đăng nhập từ bên thứ ba không hợp lệ.");
            }
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Email == externalUserInfo.Email, cancellationToken);
            if (user == null)
            {
                user = new User
                {
                    Email = externalUserInfo.Email,
                    FullName = externalUserInfo.Name,
                    IsActive = true
                };
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await _authService.AddUserSecurityLogAsync(user.Id, UserSecurityActions.ExternalRegister);
            }
            await _authService.AddUserSecurityLogAsync(user.Id, UserSecurityActions.ExternalLogin);
            var authResponse = await _authService.GenerateLoginSessionAsync(user, cancellationToken);
            return authResponse;
        }
    }
}
