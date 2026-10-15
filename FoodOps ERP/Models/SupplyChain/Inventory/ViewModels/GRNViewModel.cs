using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.ViewModels.Inventory
{
    public class GRNViewModel
    {
        public int POID { get; set; }

        [Required]
        public string POCode { get; set; } = string.Empty;

        public int SupplierID { get; set; }
        public string? SupplierName { get; set; }

        public DateTime ReceivedDate { get; set; } = DateTime.Today;

        // ✅ ADD — Remarks
        [StringLength(500)]
        public string? Remarks { get; set; }

        public List<GRNItemViewModel> Items { get; set; } = new();
    }

    public class GRNItemViewModel
    {
        public int POItemID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }

        // ✅ ADD — UOMCode
        [StringLength(20)]
        public string? UOMCode { get; set; }

        public decimal OrderedQty { get; set; }
        public decimal AlreadyReceivedQty { get; set; }

        // ✅ ADD — ReceivedQty (pehle se mila hua — display ke liye)
        public decimal ReceivedQty { get; set; }

        [Required]
        [Range(0.0001, double.MaxValue)]
        public decimal ReceivingQty { get; set; }

        // ✅ ADD — ReceiveQty as alias (Reviewer 2 ka code isko use karta hai)
        public decimal ReceiveQty
        {
            get => ReceivingQty;
            set => ReceivingQty = value;
        }

        public string? BatchNumber { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        // ✅ ADD — BinID
        public int? BinID { get; set; }
    }
}