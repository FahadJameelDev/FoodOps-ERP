using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.Costing.ViewModels;

public class StandardCostViewModel
{
    public int CostID { get; set; }

    [Required(ErrorMessage = "Product is required")]
    public int ProductID { get; set; }
    public string? ProductName { get; set; }

    [Required]
    public DateTime EffectiveFrom { get; set; } = DateTime.Today;

    public DateTime? EffectiveTo { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal StandardMaterialCost { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal StandardLaborCost { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal StandardOverheadCost { get; set; }

    public decimal TotalStandardCost { get; set; } // Computed in DB

    [Required]
    public int CostUOMID { get; set; }
    public string? CostUOMName { get; set; }

    public string Status { get; set; } = "Draft";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}