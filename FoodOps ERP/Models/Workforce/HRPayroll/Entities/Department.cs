using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class Department : AuditedEntity
{
    public int DepartmentID { get; set; }
    public int PlantID { get; set; }

    [Required, MaxLength(20)]
    public string DepartmentCode { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Navigation
    public Plant Plant { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}