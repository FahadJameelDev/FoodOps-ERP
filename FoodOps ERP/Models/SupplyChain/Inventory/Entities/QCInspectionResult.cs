
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class QCInspectionResult
    {
        [Key]
        public int ResultID { get; set; }

        public int InspectionID { get; set; }
        public int SpecID { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal MeasuredValue { get; set; }

        public bool IsPassed { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public QCInspection QCInspection { get; set; } = null!;
        public QCSpecification QCSpecification { get; set; } = null!;
    }
}