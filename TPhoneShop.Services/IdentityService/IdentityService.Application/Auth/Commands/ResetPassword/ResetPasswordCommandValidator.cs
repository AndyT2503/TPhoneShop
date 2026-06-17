using FluentValidation;

namespace IdentityService.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty()
                            .MinimumLength(6)
                            .WithName("Mật khẩu");
        }
    }
}
