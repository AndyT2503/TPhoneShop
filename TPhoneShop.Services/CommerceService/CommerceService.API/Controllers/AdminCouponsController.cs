using CommerceService.Application.Coupons.Commands.CreateCoupon;
using Microsoft.AspNetCore.Mvc;

namespace CommerceService.API.Controllers.Admin
{
    [Route("api/admin/coupons")]
    [ApiController]
    public class AdminCouponsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Permissions.CouponsCreate)]
        public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponCommand command, CancellationToken cancellationToken)
        {
            var couponId = await mediator.Send(command, cancellationToken);
            return Ok(new { Id = couponId });
        }
    }
}
