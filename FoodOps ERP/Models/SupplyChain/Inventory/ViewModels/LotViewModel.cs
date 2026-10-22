namespace FoodOpsERP.Models.ViewModels.Inventory
{
    public class LotViewModel
    {
        public long BatchID { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? SupplierName { get; set; }
        public decimal CurrentQuantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string QCStatus { get; set; } = string.Empty;
        public string? BinCode { get; set; }
        public int DaysToExpiry { get; set; }

        // ✅ YEH LINE ADD KAREIN
        // LotViewModel.cs
        public DateTime? ReceivedDate { get; set; } // ✅ '?' laga dein
    }
}