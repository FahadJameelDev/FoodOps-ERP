using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class PurchaseOrder
    {
        [Key]
        public int POID { get; set; }

        [Required, StringLength(30)]
        public string POCode { get; set; } = string.Empty;

        public int SupplierID { get; set; }
        public int PlantID { get; set; }

        [Required, StringLength(30)]
        public string Status { get; set; } = "Draft";

        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        public int? CreatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? ModifiedByUserID { get; set; }
        public DateTime? ModifiedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public Supplier Supplier { get; set; } = null!;
        public Plant Plant { get; set; } = null!;
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
    }
}