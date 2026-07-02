using FileService.Application.Common.Dtos;
using Microsoft.AspNetCore.Http;

namespace FileService.Application.File.Commands.UploadFile
{
    public class UploadFileCommand : IRequest<FileUploadResult>
    {
        public required IFormFile File { get; set; }
        public string? CustomFileName { get; set; }
    }
}
