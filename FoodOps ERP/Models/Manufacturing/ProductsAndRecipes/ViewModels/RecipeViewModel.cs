using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // ✅ Yeh add karein

namespace FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;

public class RecipeViewModel
{
    public int RecipeID { get; set; }

    [Required(ErrorMessage = "Product is required")]
    [Display(Name = "Product")]
    public int ProductID { get; set; }

    [Display(Name = "Product Name")]
    public string? ProductName { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    [Display(Name = "Version No")]
    public int VersionNo { get; set; } = 1;

    [Required(ErrorMessage = "Recipe Code is required")]
    [StringLength(30)]
    [Display(Name = "Recipe Code")]
    public string RecipeCode { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Status")]
    public string Status { get; set; } = "Draft";

    [Required]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Batch Size must be greater than 0")]
    [Display(Name = "Standard Batch Size")]
    public decimal StandardBatchSize { get; set; }

    [Required(ErrorMessage = "UOM is required")]
    [Display(Name = "Batch UOM")]
    public int BatchUOMID { get; set; }

    [Display(Name = "UOM")]
    public string? BatchUOMName { get; set; }

    [Required]
    [Range(0.01, 100, ErrorMessage = "Yield must be between 0.01 and 100")]
    [Display(Name = "Yield %")]
    public decimal YieldPercentage { get; set; }

    [Display(Name = "Effective From")]
    [DataType(DataType.Date)]
    public DateTime? EffectiveFrom { get; set; }

    [Display(Name = "Effective To")]
    [DataType(DataType.Date)]
    public DateTime? EffectiveTo { get; set; }

    public bool IsCurrent { get; set; }

    [Display(Name = "Change Summary")]
    [StringLength(500)]
    public string? ChangeSummary { get; set; }

    public List<RecipeItemViewModel> Items { get; set; } = new();

    // Display only
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime CreatedAt { get; set; }

    // ✅ Dropdowns ke liye nayi properties
    public IEnumerable<SelectListItem>? ProductList { get; set; }
    public IEnumerable<SelectListItem>? UOMList { get; set; }
}