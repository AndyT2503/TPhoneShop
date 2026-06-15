using System.Net;

namespace BuildingBlocks.Application.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}
