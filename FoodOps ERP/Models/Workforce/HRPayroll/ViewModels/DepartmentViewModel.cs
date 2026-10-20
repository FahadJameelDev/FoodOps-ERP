using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class DepartmentViewModel
{
    public int DepartmentID { get; set; }

    [Required(ErrorMessage = "Plant is required")]
    public int PlantID { get; set; }
    public string? PlantName { get; set; }

    [Required(ErrorMessage = "Department Code is required")]
    [StringLength(20)]
    public string DepartmentCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department Name is required")]
    [StringLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}