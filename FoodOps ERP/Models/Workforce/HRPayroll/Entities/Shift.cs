using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class Shift : AuditedEntity
{
    public int ShiftID { get; set; }
    public int PlantID { get; set; }

    [Required, MaxLength(20)]
    public string ShiftCode { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string ShiftName { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Plant Plant { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
}