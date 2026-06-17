namespace IdentityService.Application.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand(string ResetPasswordToken, string NewPassword) : IRequest;
}
