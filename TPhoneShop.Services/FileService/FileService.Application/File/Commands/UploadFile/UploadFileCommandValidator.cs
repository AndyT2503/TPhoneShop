namespace FileService.Application.File.Commands.UploadFile
{
    internal sealed class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        public UploadFileCommandValidator()
        {
            RuleFor(x => x.File).NotNull();
        }
    }
}
