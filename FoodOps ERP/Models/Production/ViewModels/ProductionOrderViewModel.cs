using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // ✅ Yeh add karein

namespace FoodOpsERP.Models.Manufacturing.Production.ViewModels;

public class ProductionOrderViewModel
{
    public int ProductionOrderID { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Plant is required")]
    public int PlantID { get; set; }
    public string? PlantName { get; set; }

    public int? PlanLineID { get; set; }

    [Required(ErrorMessage = "Product is required")]
    public int ProductID { get; set; }
    public string? ProductName { get; set; }

    [Required(ErrorMessage = "Recipe is required")]
    public int RecipeID { get; set; }
    public string? RecipeCode { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Target must be > 0")]
    public decimal TargetQty { get; set; }

    public decimal ProducedQty { get; set; }
    public decimal RejectedQty { get; set; }

    [Required]
    public int UOMID { get; set; }
    public string? UOMName { get; set; }

    [Range(1, 5)]
    public byte Priority { get; set; } = 3;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime PlannedStart { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime PlannedEnd { get; set; } = DateTime.Today.AddDays(1);

    public DateTime? ActualStart { get; set; }
    public DateTime? ActualEnd { get; set; }

    public string Status { get; set; } = "Planned";

    [StringLength(300)]
    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<ProductionOperationViewModel> Operations { get; set; } = new();

    // ✅ Dropdowns ke liye nayi properties
    public IEnumerable<SelectListItem>? PlantList { get; set; }
    public IEnumerable<SelectListItem>? ProductList { get; set; }
    public IEnumerable<SelectListItem>? RecipeList { get; set; }
    public IEnumerable<SelectListItem>? UOMList { get; set; }
}