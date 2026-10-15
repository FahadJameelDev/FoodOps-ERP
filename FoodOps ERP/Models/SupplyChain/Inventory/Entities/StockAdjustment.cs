
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class StockAdjustment
    {
        [Key]
        public int AdjustmentID { get; set; }

        public long BatchID { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal AdjustedQty { get; set; }

        [Required, StringLength(100)]
        public string Reason { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Remarks { get; set; }

        public int AdjustedByUserID { get; set; }
        public DateTime AdjustedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties

        public InventoryBatch InventoryBatch { get; set; } = null!;
        public User AdjustedBy { get; set; } = null!;
    }
}