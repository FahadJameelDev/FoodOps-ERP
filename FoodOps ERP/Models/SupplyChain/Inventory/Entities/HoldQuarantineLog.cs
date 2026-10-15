
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class HoldQuarantineLog
    {
        [Key]
        public int HoldID { get; set; }

        public long BatchID { get; set; }

        [Required, StringLength(300)]
        public string HoldReason { get; set; } = string.Empty;

        public int HeldByUserID { get; set; }
        public DateTime HeldAt { get; set; } = DateTime.UtcNow;

        public int? ReleasedByUserID { get; set; }
        public DateTime? ReleasedAt { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } = "OnHold";

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public InventoryBatch InventoryBatch { get; set; } = null!;
        public User HeldBy { get; set; } = null!;
        public User? ReleasedBy { get; set; }
    }
}