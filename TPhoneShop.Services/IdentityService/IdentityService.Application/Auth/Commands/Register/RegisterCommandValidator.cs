using FluentValidation;

namespace IdentityService.Application.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6).WithName("Mật khẩu");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(200).WithName("Họ tên");
        }
    }
}
