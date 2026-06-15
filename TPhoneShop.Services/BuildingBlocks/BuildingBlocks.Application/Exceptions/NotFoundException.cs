using System.Net;

namespace BuildingBlocks.Application.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
        {
        }
    }
}
