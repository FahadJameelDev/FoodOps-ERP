using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.ViewModels.Inventory
{
    public class PurchaseOrderViewModel
    {
        public int POID { get; set; }

        [Required]
        [StringLength(30)]
        public string POCode { get; set; } = string.Empty;

        [Required]
        public int SupplierID { get; set; }

        [Required]
        public int PlantID { get; set; }

        [Required]
        public string Status { get; set; } = "Draft";

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Today;

        public DateTime? ExpectedDate { get; set; }

        public decimal? TotalAmount { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        // ✅ ADD — items list for PO form + details page
        public List<PurchaseOrderItemViewModel> Items { get; set; } = new();

        // Display only
        public string? SupplierName { get; set; }
        public string? PlantName { get; set; }
    }

    // ✅ ADD — new class for PO line items
    public class PurchaseOrderItemViewModel
    {
        public int POItemID { get; set; }
        public int ProductID { get; set; }
        public int UOMID { get; set; }

        public decimal OrderedQty { get; set; }
        public decimal UnitCost { get; set; }
        public decimal ReceivedQty { get; set; }

        // Display only — for Details page
        public string? ProductName { get; set; }
        public string? UOMCode { get; set; }
    }
}