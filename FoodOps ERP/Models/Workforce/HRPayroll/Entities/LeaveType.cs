using System.ComponentModel.DataAnnotations;
using FoodOpsERP.Models.Entities.Manufacturing;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class LeaveType : AuditedEntity
{
    public int LeaveTypeID { get; set; }

    [Required, MaxLength(20)]
    public string LeaveTypeCode { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string LeaveTypeName { get; set; } = string.Empty;

    public bool IsPaid { get; set; } = true;
    public int? MaxDaysPerYear { get; set; }
    public bool CarryForward { get; set; } = false;
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}