using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // ✅ Added

namespace FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;

public class RecipeChangeRequestViewModel
{
    public int ChangeID { get; set; }

    [Display(Name = "Change No")]
    public string ChangeNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a recipe")]
    [Display(Name = "Recipe")]
    public int RecipeID { get; set; }

    [Display(Name = "Recipe Code")]
    public string? RecipeCode { get; set; }

    public int? NewRecipeID { get; set; }

    [Required]
    [Display(Name = "Change Type")]
    public string ChangeType { get; set; } = "Other";

    [Required(ErrorMessage = "Reason is required")]
    [StringLength(500)]
    public string Reason { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Impact Assessment")]
    public string? ImpactAssessment { get; set; }

    [Display(Name = "Status")]
    public string Status { get; set; } = "Requested";

    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? ImplementedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // ✅ Strongly-typed collection for SPA AJAX Dropdown
    public IEnumerable<SelectListItem>? RecipeList { get; set; }
}