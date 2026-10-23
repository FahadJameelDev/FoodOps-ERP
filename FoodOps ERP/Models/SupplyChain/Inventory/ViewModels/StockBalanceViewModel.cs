namespace FoodOpsERP.Models.ViewModels.Inventory
{
    public class StockBalanceViewModel
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;

        // Service se yeh aayega
        public string? UOM { get; set; }

        // ✅ View ko yeh chahiye tha, isliye map kar diya
        public string? UOMCode => UOM;

        public decimal TotalQuantity { get; set; }
        public decimal MinStockLevel { get; set; }
        public string StockStatus { get; set; } = string.Empty; // OK / Low / Out
    }
}