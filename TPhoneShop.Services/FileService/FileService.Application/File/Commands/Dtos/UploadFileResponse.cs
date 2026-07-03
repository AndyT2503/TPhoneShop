namespace FileService.Application.File.Commands.Dtos
{
    public class UploadFileResponse
    {
        public Guid MediaId { get; set; }
        public required string PreSignedUrl { get; set; }
    }
}
