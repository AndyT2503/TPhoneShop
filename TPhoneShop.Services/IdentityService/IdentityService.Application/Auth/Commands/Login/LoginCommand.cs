using IdentityService.Application.Auth.Dtos;

namespace IdentityService.Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;
}
