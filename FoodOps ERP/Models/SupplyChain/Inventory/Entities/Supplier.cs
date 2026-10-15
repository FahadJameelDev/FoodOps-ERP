using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }

        [Required, StringLength(20)]
        public string SupplierCode { get; set; } = string.Empty;

        [Required, StringLength(150)]
        public string SupplierName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        public decimal? Rating { get; set; }

        public bool IsActive { get; set; } = true;

        public int? CreatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? ModifiedByUserID { get; set; }
        public DateTime? ModifiedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        public ICollection<InventoryBatch> InventoryBatches { get; set; } = new List<InventoryBatch>();
    }
}