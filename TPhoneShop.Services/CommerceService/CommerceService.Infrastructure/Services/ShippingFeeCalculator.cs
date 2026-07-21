using BuildingBlocks.Domain.Exceptions;
using CommerceService.Application.Common.Abstractions;
using CommerceService.Domain.Constants;
using CommerceService.Domain.ValueObjects;

namespace CommerceService.Infrastructure.Services
{
    internal class ShippingFeeCalculator : IShippingFeeCalculator
    {
        private static readonly Money StandardFee = new(30_000_00); // 30,000 VND (×100)

        public Money Calculate(string shippingMethod, ShippingAddress address)
        {
            return shippingMethod switch
            {
                ShippingMethods.Pickup => Money.Zero,
                ShippingMethods.Standard => StandardFee,
                _ => throw new DomainException("Phương thức vận chuyển không hợp lệ.")
            };
        }
    }
}
