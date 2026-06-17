using IdentityService.Application.Auth.Dtos;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Auth.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> GenerateLoginSessionAsync(User user, CancellationToken cancellationToken);
        string HashPassword(string plainPassword);
        bool VerifyPassword(string plainPassword, string hashPassword);
        string HashToken(string token);
    }
}
