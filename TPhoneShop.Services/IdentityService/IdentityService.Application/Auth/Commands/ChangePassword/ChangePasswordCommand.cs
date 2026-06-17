using IdentityService.Application.Auth.Dtos;

namespace IdentityService.Application.Auth.Commands.ChangePassword
{
    public record ChangePasswordCommand(string NewPassword, string OldPassword) : IRequest<AuthResponse>;
}
