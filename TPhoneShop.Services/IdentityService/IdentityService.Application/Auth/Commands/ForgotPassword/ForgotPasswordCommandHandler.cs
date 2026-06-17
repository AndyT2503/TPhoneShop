using IdentityService.Application.Auth.Services;
using IdentityService.Application.Common.Constants;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IdentityService.Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IAuthService _authService;
        public ForgotPasswordCommandHandler(MainDbContext mainDbContext, IAuthService authService)
        {
            _mainDbContext = mainDbContext;
            _authService = authService;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _mainDbContext.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Email == request.Email, cancellationToken);
            if (user is null)
            {
                return;
            }
            var token = Guid.NewGuid().ToString("N");
            var resetPasswordToken = new ResetPasswordToken
            {
                UserId = user.Id,
                Token = _authService.HashToken(token),
                ExpiredAt = DateTimeOffset.UtcNow.AddMinutes(15),
                IsUsed = false
            };
            _mainDbContext.ResetPasswordTokens.Add(resetPasswordToken);
            _mainDbContext.OutboxMessages.Add(new OutboxMessage
            {
                Type = OutboxEventTypes.ForgotPassword,
                Payload = JsonSerializer.SerializeToDocument(new
                {
                    recipientId = user.Id,
                    email = user.Email,
                    token
                }),
                ExpiresAt = resetPasswordToken.ExpiredAt,
            });
            await _mainDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
