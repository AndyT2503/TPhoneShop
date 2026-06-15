namespace IdentityService.Application.Auth.Services
{
    public interface IJwksService
    {
        Task<object> GetJwks();
    }
}
