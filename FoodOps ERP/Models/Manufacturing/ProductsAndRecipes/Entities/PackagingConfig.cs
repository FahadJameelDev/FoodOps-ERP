using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Entities.Manufacturing;

public class PackagingConfig : AuditedEntity
{
    public int PackagingConfigID { get; set; }
    public int ProductID { get; set; }

    [Required, MaxLength(30)]
    public string PackageType { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string PackageName { get; set; } = string.Empty;

    public int UnitsPerPack { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal NetWeight { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal GrossWeight { get; set; }

    public int WeightUOMID { get; set; }

    [MaxLength(50)]
    public string? Barcode { get; set; }

    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Product Product { get; set; } = null!; // Assuming Product is in global namespace
}