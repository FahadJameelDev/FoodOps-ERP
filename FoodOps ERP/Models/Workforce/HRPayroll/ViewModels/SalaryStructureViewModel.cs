using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class SalaryStructureViewModel
{
    public int StructureID { get; set; }

    [Required(ErrorMessage = "Structure Code is required")]
    [StringLength(30)]
    public string StructureCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Structure Name is required")]
    [StringLength(100)]
    public string StructureName { get; set; } = string.Empty;

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

    public bool IsActive { get; set; } = true;
}