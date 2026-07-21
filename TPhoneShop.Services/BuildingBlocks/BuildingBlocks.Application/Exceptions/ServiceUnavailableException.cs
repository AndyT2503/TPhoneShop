using System.Net;

namespace BuildingBlocks.Application.Exceptions
{
    public class ServiceUnavailableException : AppException
    {
        public ServiceUnavailableException(string message) : base(message, HttpStatusCode.ServiceUnavailable)
        {
        }
    }
}
