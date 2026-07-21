using System.Reflection;

namespace CommerceService.Domain.Constants
{
    public static class ShippingMethods
    {
        public const string Standard = "Standard";
        public const string Pickup = "Pickup";

        public static readonly IReadOnlyList<string> All =
            typeof(ShippingMethods)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && f.FieldType == typeof(string))
                .Select(f => (string)f.GetRawConstantValue()!)
                .ToList();
    }
}
