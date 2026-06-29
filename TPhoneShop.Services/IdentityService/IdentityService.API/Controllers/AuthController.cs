using IdentityService.API.Extensions;
using IdentityService.Application.Auth.Commands.ChangePassword;
using IdentityService.Application.Auth.Commands.ExternalLogin;
using IdentityService.Application.Auth.Commands.ForgotPassword;
using IdentityService.Application.Auth.Commands.Login;
using IdentityService.Application.Auth.Commands.Logout;
using IdentityService.Application.Auth.Commands.RefreshToken;
using IdentityService.Application.Auth.Commands.Register;
using IdentityService.Application.Auth.Commands.ResetPassword;
using IdentityService.Application.Auth.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            Response.SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresTime);
            return Ok(new
            {
                result.AccessToken
            });
        }

        [HttpPost("external-login")]
        public async Task<IActionResult> ExternalLogin(ExternalLoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            Response.SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresTime);
            return Ok(new
            {
                result.AccessToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            Response.SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresTime);
            return Ok(new
            {
                result.AccessToken
            });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            Response.SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresTime);
            return Ok(new
            {
                result.AccessToken
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            var refreshToken = Request.GetRefreshTokenCookie();
            var result = await _mediator.Send(new RefreshTokenCommand(refreshToken), cancellationToken);

            Response.SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresTime);
            return Ok(new
            {
                result.AccessToken,
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var refreshToken = Request.GetRefreshTokenCookie();
            await _mediator.Send(new LogoutCommand(refreshToken), cancellationToken);
            return Ok();
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser([FromQuery] GetUserProfileQuery query, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(query, cancellationToken));
        }
    }
}
