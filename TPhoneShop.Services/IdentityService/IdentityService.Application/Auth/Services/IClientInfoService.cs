namespace IdentityService.Application.Auth.Services
{
    public interface IClientInfoService
    {
        string GetDeviceName();
        string GetIPAddress();
        string GetUserAgent();
    }
}
