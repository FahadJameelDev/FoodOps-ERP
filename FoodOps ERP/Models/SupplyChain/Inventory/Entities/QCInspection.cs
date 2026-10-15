
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class QCInspection
    {
        [Key]
        public int InspectionID { get; set; }

        public long BatchID { get; set; }
        public int? ProductionOrderID { get; set; }
        public int InspectorUserID { get; set; }

        [Required, StringLength(30)]
        public string InspectionType { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string OverallResult { get; set; } = string.Empty;

        public DateTime InspectionDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Remarks { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public InventoryBatch InventoryBatch { get; set; } = null!;
        public User Inspector { get; set; } = null!;
        public ICollection<QCInspectionResult> QCInspectionResults { get; set; } = new List<QCInspectionResult>();
        public ICollection<NonConformanceReport> NonConformanceReports { get; set; } = new List<NonConformanceReport>();
    }
}