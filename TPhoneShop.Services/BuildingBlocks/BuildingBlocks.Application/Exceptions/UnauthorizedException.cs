using System.Net;

namespace BuildingBlocks.Application.Exceptions
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message) : base(message, HttpStatusCode.Unauthorized)
        {
        }
    }
}
