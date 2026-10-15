
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class InventoryTransaction
    {
        [Key]
        public long TransactionID { get; set; }

        public long BatchID { get; set; }
        public int PlantID { get; set; }

        [Required, StringLength(30)]
        public string TransactionType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; }

        [StringLength(30)]
        public string? ReferenceType { get; set; }

        public int? ReferenceID { get; set; }

        [StringLength(300)]
        public string? Remarks { get; set; }

        public int? CreatedByUserID { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public InventoryBatch InventoryBatch { get; set; } = null!;
        public Plant Plant { get; set; } = null!;
    }
}