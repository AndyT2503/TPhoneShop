using BuildingBlocks.Domain.Entities;

namespace FileService.Domain.Entities
{
    public class Media : BaseEntity
    {
        public required string Key { get; set; }
        public string? ReferrenceId { get; set; }
        public long Size { get; set; }
        public required string ContentType { get; set; }
    }
}
