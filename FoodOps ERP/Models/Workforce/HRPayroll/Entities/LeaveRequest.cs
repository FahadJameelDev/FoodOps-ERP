using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public static class LeaveRequestStatus
{
    public const string Pending = "Pending";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Cancelled = "Cancelled";
    public static readonly string[] All = { Pending, Approved, Rejected, Cancelled };
}

public class LeaveRequest : AuditedEntity
{
    public int LeaveRequestID { get; set; }

    [Required, MaxLength(30)]
    public string RequestNo { get; set; } = string.Empty;

    public int EmployeeID { get; set; }
    public int LeaveTypeID { get; set; }

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    [Column(TypeName = "decimal(5,1)")]
    public decimal TotalDays { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = LeaveRequestStatus.Pending;

    public int? ApprovedByUserID { get; set; }
    public DateTime? ApprovedAt { get; set; }

    [MaxLength(300)]
    public string? RejectionReason { get; set; }

    // Navigation
    public Employee Employee { get; set; } = null!;
    public LeaveType LeaveType { get; set; } = null!;
    public User? ApprovedByUser { get; set; }
}