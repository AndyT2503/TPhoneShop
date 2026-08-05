namespace CommerceService.Application.Orders.Commands.ConfirmOrderPayment
{
    public class ConfirmOrderPaymentCommand : IRequest
    {
        public Guid OrderId { get; set; }
    }
}
