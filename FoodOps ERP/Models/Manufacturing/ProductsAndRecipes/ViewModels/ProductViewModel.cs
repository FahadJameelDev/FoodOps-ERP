using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;

public class ProductViewModel
{
    public int ProductID { get; set; }

    [Required(ErrorMessage = "Product Code is required")]
    [StringLength(20)]
    public string ProductCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product Name is required")]
    [StringLength(150)]
    public string ProductName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required")]
    public int CategoryID { get; set; }

    [Required(ErrorMessage = "Base UOM is required")]
    public int BaseUOMID { get; set; }

    [Required]
    [StringLength(30)]
    public string ProductType { get; set; } = "Finished"; // Finished, RawMaterial, etc.

    public bool IsActive { get; set; } = true;

    // Display only
    public string? CategoryName { get; set; }
    public string? UOMName { get; set; }
}