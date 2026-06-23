using FluentValidation;

namespace IdentityService.Application.Auth.Commands.ChangePassword
{
    internal class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.NewPassword)
                    .NotEmpty()
                    .MinimumLength(6).WithName("Mật khẩu");
        }
    }
}
