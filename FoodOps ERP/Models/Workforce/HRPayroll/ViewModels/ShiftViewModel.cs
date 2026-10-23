using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // ✅ Yeh add karein

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class ShiftViewModel
{
    public int ShiftID { get; set; }

    [Required(ErrorMessage = "Plant is required")]
    public int PlantID { get; set; }
    public string? PlantName { get; set; }

    [Required(ErrorMessage = "Shift Code is required")]
    [StringLength(20)]
    public string ShiftCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Shift Name is required")]
    [StringLength(50)]
    public string ShiftName { get; set; } = string.Empty;

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    public bool IsActive { get; set; } = true;

    // ✅ Dropdown ke liye nayi property
    public IEnumerable<SelectListItem>? PlantList { get; set; }
}