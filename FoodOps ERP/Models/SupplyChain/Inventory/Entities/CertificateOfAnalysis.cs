
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class CertificateOfAnalysis
    {
        [Key]
        public int COAID { get; set; }

        public long BatchID { get; set; }

        [Required, StringLength(50)]
        public string COANumber { get; set; } = string.Empty;

        [StringLength(150)]
        public string? IssuedToCustomer { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public int? GeneratedByUserID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public InventoryBatch InventoryBatch { get; set; } = null!;
    }
}