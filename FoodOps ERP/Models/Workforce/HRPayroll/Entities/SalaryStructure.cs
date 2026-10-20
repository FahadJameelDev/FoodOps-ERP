using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoodOpsERP.Models.Entities.Manufacturing;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class SalaryStructure : AuditedEntity
{
    public int StructureID { get; set; }

    [Required, MaxLength(30)]
    public string StructureCode { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string StructureName { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasicSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal HousingAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TransportAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OtherAllowance { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<EmployeeSalary> EmployeeSalaries { get; set; } = new List<EmployeeSalary>();
}