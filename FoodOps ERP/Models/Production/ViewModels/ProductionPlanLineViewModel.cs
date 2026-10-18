using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.Production.ViewModels;

public class ProductionPlanLineViewModel
{
    public int PlanLineID { get; set; }
    public int PlanID { get; set; }

    public int LineNo { get; set; }

    [Required(ErrorMessage = "Product is required")]
    public int ProductID { get; set; }
    public string? ProductName { get; set; }

    [Required(ErrorMessage = "Recipe is required")]
    public int RecipeID { get; set; }
    public string? RecipeCode { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue)]
    public decimal PlannedQty { get; set; }

    [Required]
    public int UOMID { get; set; }
    public string? UOMName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime PlannedDate { get; set; } = DateTime.Today;

    public int? DemandID { get; set; }
    public string? DemandCode { get; set; }

    [Range(1, 5)]
    public byte Priority { get; set; } = 3;

    public string Status { get; set; } = "Planned";
}