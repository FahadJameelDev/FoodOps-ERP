using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // ✅ Yeh add karein

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class EmployeeSalaryViewModel
{
    public int EmployeeSalaryID { get; set; }

    [Required(ErrorMessage = "Employee is required")]
    public int EmployeeID { get; set; }
    public string? EmployeeName { get; set; }

    public int? StructureID { get; set; }
    public string? StructureName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime EffectiveFrom { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime? EffectiveTo { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal BasicSalary { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal HousingAllowance { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal TransportAllowance { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal OtherAllowance { get; set; }

    public decimal GrossSalary { get; set; }
    public string Status { get; set; } = "Active";

    // ✅ Dropdowns ke liye nayi properties
    public IEnumerable<SelectListItem>? EmployeeList { get; set; }
    public IEnumerable<SelectListItem>? StructureList { get; set; }
}