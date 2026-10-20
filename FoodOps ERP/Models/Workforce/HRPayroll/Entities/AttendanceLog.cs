using System.ComponentModel.DataAnnotations;
using FoodOpsERP.Models.Entities.Manufacturing;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public static class AttendanceStatus
{
    public const string Present = "Present";
    public const string Absent = "Absent";
    public const string Late = "Late";
    public const string HalfDay = "HalfDay";
    public const string Holiday = "Holiday";
    public const string Leave = "Leave";
    public static readonly string[] All = { Present, Absent, Late, HalfDay, Holiday, Leave };
}

public class AttendanceLog : VersionedEntity
{
    public long AttendanceID { get; set; }
    public int EmployeeID { get; set; }
    public int? ShiftID { get; set; }

    public DateTime LogDate { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = AttendanceStatus.Present;

    public int? WorkedMinutes { get; set; }

    [MaxLength(200)]
    public string? Remarks { get; set; }

    // Navigation
    public Employee Employee { get; set; } = null!;
    public Shift? Shift { get; set; }
    public ICollection<AttendanceCorrection> Corrections { get; set; } = new List<AttendanceCorrection>();
}