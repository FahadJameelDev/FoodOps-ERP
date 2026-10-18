using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.Production.ViewModels;

public class DemandViewModel
{
    public int DemandID { get; set; }

    [Display(Name = "Demand Code")]
    public string DemandCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Plant is required")]
    public int PlantID { get; set; }
    public string? PlantName { get; set; }

    [Required(ErrorMessage = "Product is required")]
    public int ProductID { get; set; }
    public string? ProductName { get; set; }

    [Required(ErrorMessage = "UOM is required")]
    public int UOMID { get; set; }
    public string? UOMName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime RequiredDate { get; set; } = DateTime.Today.AddDays(7);

    [Required]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantity must be > 0")]
    public decimal Quantity { get; set; }

    [Required]
    public string Source { get; set; } = "Manual";

    [StringLength(60)]
    public string? ReferenceNo { get; set; }

    [Range(1, 5)]
    public byte Priority { get; set; } = 3;

    public string Status { get; set; } = "Open";

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
}