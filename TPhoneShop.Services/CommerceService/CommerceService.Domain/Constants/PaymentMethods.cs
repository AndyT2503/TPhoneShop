using System.Reflection;

namespace CommerceService.Domain.Constants
{
    public static class PaymentMethods
    {
        public const string CashOnDelivery = "CashOnDelivery";
        public const string Stripe = "Stripe";
        public const string CashOnPickup = "CashOnPickup";

        public static readonly IReadOnlyList<string> All =
            typeof(PaymentMethods)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && f.FieldType == typeof(string))
                .Select(f => (string)f.GetRawConstantValue()!)
                .ToList();

        public static bool IsOfflinePayment(string method)
        {
            return method is CashOnDelivery or CashOnPickup;
        }
    }
}
