using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoodOpsERP.Models.Entities.Manufacturing;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class EmployeeSalary : AuditedEntity
{
    public int EmployeeSalaryID { get; set; }
    public int EmployeeID { get; set; }
    public int? StructureID { get; set; }

    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasicSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal HousingAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TransportAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OtherAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossSalary { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Active"; // Active / Superseded

    // Navigation
    public Employee Employee { get; set; } = null!;
    public SalaryStructure? Structure { get; set; }
}