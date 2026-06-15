using IdentityService.Application.Auth.Dtos;

namespace IdentityService.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponse>;
}
