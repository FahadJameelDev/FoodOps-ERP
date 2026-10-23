using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // ✅ Yeh add karein

namespace FoodOpsERP.Models.Manufacturing.Quality.ViewModels;

public class QCParameterViewModel
{
    public int ParameterID { get; set; }

    [Required(ErrorMessage = "Parameter Code is required")]
    [StringLength(20)]
    public string ParameterCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parameter Name is required")]
    [StringLength(100)]
    public string ParameterName { get; set; } = string.Empty;

    [Required]
    public string ParameterType { get; set; } = "Numeric";

    public int? DefaultUOMID { get; set; }
    public string? DefaultUOMName { get; set; }

    public bool IsActive { get; set; } = true;

    // ✅ Dropdown ke liye nayi property
    public IEnumerable<SelectListItem>? UOMList { get; set; }
}