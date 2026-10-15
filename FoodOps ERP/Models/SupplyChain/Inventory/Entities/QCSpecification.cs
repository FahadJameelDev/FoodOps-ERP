using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities
{
    public class QCSpecification
    {
        [Key]
        public int SpecID { get; set; }

        public int ProductID { get; set; }
        public int? UOMID { get; set; }

        [Required, StringLength(100)]
        public string ParameterName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,4)")]
        public decimal? MinValue { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? MaxValue { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? TargetValue { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public Product Product { get; set; } = null!;
        public UnitOfMeasure? UOM { get; set; }
        public ICollection<QCInspectionResult> QCInspectionResults { get; set; } = new List<QCInspectionResult>();
    }
}