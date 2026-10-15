
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class NonConformanceReport
    {
        [Key]
        public int NCRID { get; set; }

        public int InspectionID { get; set; }

        [Required, StringLength(500)]
        public string DefectDescription { get; set; } = string.Empty;

        [StringLength(500)]
        public string? RootCause { get; set; }

        [StringLength(500)]
        public string? CorrectiveAction { get; set; }

        [Required, StringLength(30)]
        public string Status { get; set; } = "Open";

        public int CreatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public QCInspection QCInspection { get; set; } = null!;
    }
}