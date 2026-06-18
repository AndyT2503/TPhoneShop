using IdentityService.Application.Auth.Services;
using IdentityService.Application.Common.Constants;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IdentityService.Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IAuthService _authService;
        public ForgotPasswordCommandHandler(IdentityDbContext dbContext, IAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Email == request.Email, cancellationToken);
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
            _dbContext.ResetPasswordTokens.Add(resetPasswordToken);
            _dbContext.OutboxMessages.Add(new OutboxMessage
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
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
