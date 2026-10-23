using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // ✅ Added

namespace FoodOpsERP.Models.Manufacturing.Production.ViewModels;

public class ProductionPlanViewModel
{
    public int PlanID { get; set; }

    public string PlanCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Plant is required")]
    public int PlantID { get; set; }
    public string? PlantName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

    public string Status { get; set; } = "Draft";

    [StringLength(500)]
    public string? Notes { get; set; }

    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<ProductionPlanLineViewModel> Lines { get; set; } = new();

    // ✅ Strongly-typed dropdown lists
    public IEnumerable<SelectListItem>? PlantList { get; set; }
    public IEnumerable<SelectListItem>? ProductList { get; set; }
    public IEnumerable<SelectListItem>? RecipeList { get; set; }
    public IEnumerable<SelectListItem>? UOMList { get; set; }
    public IEnumerable<SelectListItem>? DemandList { get; set; }
}