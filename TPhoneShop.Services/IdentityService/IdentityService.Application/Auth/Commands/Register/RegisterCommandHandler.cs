using IdentityService.Application.Auth.Dtos;
using IdentityService.Application.Auth.Services;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Application.Auth.Commands.Register
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IAuthService _authService;
        public RegisterCommandHandler(MainDbContext mainDbContext, IAuthService authService)
        {
            _mainDbContext = mainDbContext;
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _mainDbContext.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

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
            _mainDbContext.Users.Add(user);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            var authResponse = await _authService.GenerateLoginSessionAsync(user, cancellationToken);
            return authResponse;
        }
    }
}
