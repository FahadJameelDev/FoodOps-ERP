using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;

public class RecipeItemViewModel
{
    public int RecipeItemID { get; set; }
    public int RecipeID { get; set; }

    [Required]
    [Display(Name = "Line No")]
    public int LineNo { get; set; }

    [Required(ErrorMessage = "Ingredient is required")]
    [Display(Name = "Ingredient")]
    public int IngredientProductID { get; set; }

    [Display(Name = "Ingredient Name")]
    public string? IngredientName { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue)]
    [Display(Name = "Required Qty")]
    public decimal RequiredQty { get; set; }

    [Required]
    [Display(Name = "UOM")]
    public int UOMID { get; set; }

    [Display(Name = "UOM Name")]
    public string? UOMName { get; set; }

    [Range(0, 100)]
    [Display(Name = "Scrap %")]
    public decimal ScrapPercent { get; set; }

    [Display(Name = "Critical")]
    public bool IsCritical { get; set; }

    [StringLength(200)]
    public string? Remarks { get; set; }
}