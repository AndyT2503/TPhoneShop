using CommerceService.Application.Common.Abstractions;

namespace CommerceService.Infrastructure.Services
{
    internal class OrderNumberGenerator : IOrderNumberGenerator
    {
        /// <summary>
        /// Generates order number in format: TPS-yyMMdd-6HEX
        /// Example: TPS-260727-A3F2B1
        /// </summary>
        public string Generate()
        {
            var datePart = DateTimeOffset.UtcNow.ToString("yyMMdd");
            var hashPart = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
            return $"TPS-{datePart}-{hashPart}";
        }
    }
}
