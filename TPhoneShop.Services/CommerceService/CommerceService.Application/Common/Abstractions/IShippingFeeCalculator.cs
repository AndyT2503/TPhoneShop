using CommerceService.Domain.ValueObjects;

namespace CommerceService.Application.Common.Abstractions
{
    public interface IShippingFeeCalculator
    {
        Money Calculate(string shippingMethod, ShippingAddress address);
    }
}
