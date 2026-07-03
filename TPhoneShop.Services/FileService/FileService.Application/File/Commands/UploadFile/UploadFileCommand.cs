using FileService.Application.File.Commands.Dtos;
using Microsoft.AspNetCore.Http;

namespace FileService.Application.File.Commands.UploadFile
{
    public class UploadFileCommand : IRequest<UploadFileResponse>
    {
        public required IFormFile File { get; set; }
        public string? CustomFileName { get; set; }
    }
}
