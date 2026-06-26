using BuildingBlocks.Domain.Constants;
using System.Text.Json;

namespace BuildingBlocks.Domain.Entities
{
    public class OutboxMessageBase : BaseEntity
    {
        public required string Type { get; set; }
        public required JsonDocument Payload { get; set; }
        public string Status { get; set; } = OutboxStatus.Pending;
        public int RetryCount { get; set; }
        public DateTimeOffset? ProcessedAt { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public string? Error { get; set; }
    }
}
