
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class InventoryBatch
    {
        [Key]
        public long BatchID { get; set; }

        [Required, StringLength(50)]
        public string BatchNumber { get; set; } = string.Empty;

        public int ProductID { get; set; }
        public int? SupplierID { get; set; }
        public int? BinID { get; set; }
        public int PlantID { get; set; }

        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal CurrentQuantity { get; set; } = 0;

        [Required, StringLength(30)]
        public string QCStatus { get; set; } = "Pending";

        public DateTime? ReceivedDate { get; set; }

        public int? CreatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? ModifiedByUserID { get; set; }
        public DateTime? ModifiedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public Product Product { get; set; } = null!;
        public Plant Plant { get; set; } = null!;
        public Supplier? Supplier { get; set; }
        public WarehouseBin? WarehouseBin { get; set; }
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
        public ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();
        public ICollection<QCInspection> QCInspections { get; set; } = new List<QCInspection>();
        public ICollection<HoldQuarantineLog> HoldQuarantineLogs { get; set; } = new List<HoldQuarantineLog>();
        public ICollection<CertificateOfAnalysis> CertificatesOfAnalysis { get; set; } = new List<CertificateOfAnalysis>();
    }
}