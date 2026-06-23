using IdentityService.Application.Auth.Dtos;

namespace IdentityService.Application.Auth.Commands.ExternalLogin
{
    public record ExternalLoginCommand(string IdToken) : IRequest<AuthResponse>;
}
