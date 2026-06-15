using IdentityService.API.Extensions;
using IdentityService.Application.Auth.Commands.Login;
using IdentityService.Application.Auth.Commands.Logout;
using IdentityService.Application.Auth.Commands.RefreshToken;
using IdentityService.Application.Auth.Commands.Register;
using MediatR;
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
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);

            Response.SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresTime);
            return Ok(new
            {
                result.AccessToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            Response.SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresTime);
            return Ok(new
            {
                result.AccessToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.GetRefreshTokenCookie();
            var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));

            Response.SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresTime);
            return Ok(new
            {
                result.AccessToken,
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.GetRefreshTokenCookie();
            await _mediator.Send(new LogoutCommand(refreshToken));
            return Ok();
        }
    }
}
