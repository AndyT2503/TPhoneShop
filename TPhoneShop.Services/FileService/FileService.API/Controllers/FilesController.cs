using FileService.Application.File.Commands.UploadFile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FileService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
