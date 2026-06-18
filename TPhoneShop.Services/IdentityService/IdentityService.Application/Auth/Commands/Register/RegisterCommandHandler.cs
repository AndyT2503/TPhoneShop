using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Constants;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.Register
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IAuthService _authService;
        public RegisterCommandHandler(IdentityDbContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (existingUser)
            {
                throw new BadRequestException("Email đã tồn tại");
            }
            var passwordHash = _authService.HashPassword(request.Password);
            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                FullName = request.FullName,
                IsActive = true
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _authService.AddUserSecurityLogAsync(user.Id, UserSecurityActions.Register);
            var authResponse = await _authService.GenerateLoginSessionAsync(user, cancellationToken);
            return authResponse;
        }
    }
}
