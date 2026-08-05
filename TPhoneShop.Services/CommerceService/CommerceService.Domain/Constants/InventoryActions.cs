namespace CommerceService.Domain.Constants
{
    public static class InventoryActions
    {
        public const string Purchase = "PURCHASE";
        public const string OrderPlaced = "ORDER_PLACED";
        public const string OrderCancelled = "ORDER_CANCELLED";
        public const string OrderReturned = "ORDER_RETURNED";
        public const string ManualAdjustment = "MANUAL_ADJUSTMENT";
        public const string StockTake = "STOCK_TAKE";
    }
}
