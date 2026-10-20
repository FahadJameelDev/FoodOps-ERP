using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public static class EmploymentStatuses
{
    public const string Active = "Active";
    public const string Resigned = "Resigned";
    public const string Terminated = "Terminated";
    public static readonly string[] All = { Active, Resigned, Terminated };
}

public class Employee : AuditedEntity
{
    public int EmployeeID { get; set; }
    public int PlantID { get; set; }
    public int DepartmentID { get; set; }
    public int? ShiftID { get; set; }
    public int? UserID { get; set; }

    [Required, MaxLength(30)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(15)]
    public string? CNIC { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(100)]
    public string? Designation { get; set; }

    public DateTime JoiningDate { get; set; }
    public DateTime? LeavingDate { get; set; }

    [Required, MaxLength(20)]
    public string EmploymentStatus { get; set; } = EmploymentStatuses.Active;

    [MaxLength(30)]
    public string? BankAccountNo { get; set; }

    [MaxLength(100)]
    public string? BankName { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public Plant Plant { get; set; } = null!;
    public Department Department { get; set; } = null!;
    public Shift? Shift { get; set; }
    public User? User { get; set; }

    public ICollection<EmployeeDocument> Documents { get; set; } = new List<EmployeeDocument>();
    public ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
    public ICollection<OvertimeRecord> OvertimeRecords { get; set; } = new List<OvertimeRecord>();
    public ICollection<EmployeeSalary> Salaries { get; set; } = new List<EmployeeSalary>();
    public ICollection<PayrollLine> PayrollLines { get; set; } = new List<PayrollLine>();
}