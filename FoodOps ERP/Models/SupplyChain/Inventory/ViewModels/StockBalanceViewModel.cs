namespace FoodOpsERP.Models.ViewModels.Inventory
{
    public class StockBalanceViewModel
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? UOM { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal MinStockLevel { get; set; }
        public string StockStatus { get; set; } = string.Empty; // OK / Low / Out
    }
}