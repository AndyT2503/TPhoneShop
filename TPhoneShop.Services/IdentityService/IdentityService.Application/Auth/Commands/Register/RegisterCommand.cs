using IdentityService.Application.Auth.Dtos;

namespace IdentityService.Application.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<AuthResponse>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FullName { get; set; }
    }
}
