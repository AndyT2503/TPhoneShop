using IdentityService.Application.Auth.Services;
using Microsoft.AspNetCore.Http;
using UAParser;

namespace IdentityService.Infrastructure.Securities
{
    internal class ClientInfoService : IClientInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientInfoService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetIPAddress()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
            {
                return "Unknown IP";
            }

            return context.Connection.RemoteIpAddress?.ToString()
                   ?? "Unknown IP";
        }

        public string GetUserAgent()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
            {
                return "Unknown UA";
            }

            return context.Request.Headers.UserAgent.ToString();
        }

        public string GetDeviceName()
        {
            var userAgent = GetUserAgent();

            if (string.IsNullOrWhiteSpace(userAgent))
            {
                return "Unknown";
            }
            var client = Parser
                        .GetDefault()
                        .Parse(GetUserAgent());

            var os = client.OS.Family;
            var browser = client.UA.Family;

            if (os == "Other" && browser == "Other")
            {
                return "Unknown Device";
            }

            return $"{os} - {browser}";
        }
    }
}
