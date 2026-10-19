using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.Costing.ViewModels;

public class OverheadAllocationViewModel
{
    public int AllocationID { get; set; }

    [Required(ErrorMessage = "Plant is required")]
    public int PlantID { get; set; }
    public string? PlantName { get; set; }

    [Required(ErrorMessage = "Component is required")]
    public int ComponentID { get; set; }
    public string? ComponentName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime PeriodMonth { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

    [Required]
    [Range(0, double.MaxValue)]
    public decimal AllocatedAmount { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue)]
    public decimal BasisQty { get; set; }

    public decimal RatePerBasis { get; set; } // Computed

    public string Status { get; set; } = "Draft";
}