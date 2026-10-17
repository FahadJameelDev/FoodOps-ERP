using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;

public class PackagingConfigViewModel
{
    public int PackagingConfigID { get; set; }

    [Required(ErrorMessage = "Product is required")]
    [Display(Name = "Product")]
    public int ProductID { get; set; }

    [Display(Name = "Product Name")]
    public string? ProductName { get; set; }

    [Required(ErrorMessage = "Package Type is required")]
    [StringLength(30)]
    [Display(Name = "Package Type")]
    public string PackageType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Package Name is required")]
    [StringLength(100)]
    [Display(Name = "Package Name")]
    public string PackageName { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    [Display(Name = "Units Per Pack")]
    public int UnitsPerPack { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue)]
    [Display(Name = "Net Weight")]
    public decimal NetWeight { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue)]
    [Display(Name = "Gross Weight")]
    public decimal GrossWeight { get; set; }

    [Required]
    [Display(Name = "Weight UOM")]
    public int WeightUOMID { get; set; }

    [Display(Name = "UOM")]
    public string? WeightUOMName { get; set; }

    [StringLength(50)]
    public string? Barcode { get; set; }

    [Display(Name = "Default")]
    public bool IsDefault { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}