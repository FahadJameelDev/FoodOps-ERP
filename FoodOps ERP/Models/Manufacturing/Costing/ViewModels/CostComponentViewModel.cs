using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.Costing.ViewModels;

public class CostComponentViewModel
{
    public int ComponentID { get; set; }

    [Required(ErrorMessage = "Component Code is required")]
    [StringLength(20)]
    public string ComponentCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Component Name is required")]
    [StringLength(100)]
    public string ComponentName { get; set; } = string.Empty;

    [Required]
    public string ComponentType { get; set; } = "Overhead";

    [Required]
    public string AllocationBasis { get; set; } = "PerBatch";

    public bool IsActive { get; set; } = true;
}