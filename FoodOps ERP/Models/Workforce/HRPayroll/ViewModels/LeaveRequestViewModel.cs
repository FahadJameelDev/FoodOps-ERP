using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class LeaveRequestViewModel
{
    public int LeaveRequestID { get; set; }
    public string RequestNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Employee is required")]
    public int EmployeeID { get; set; }
    public string? EmployeeName { get; set; }

    [Required(ErrorMessage = "Leave Type is required")]
    public int LeaveTypeID { get; set; }
    public string? LeaveTypeName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime FromDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Date)]
    public DateTime ToDate { get; set; } = DateTime.Today;

    [Required]
    [Range(0.5, 365)]
    public decimal TotalDays { get; set; }

    [StringLength(500)]
    public string? Reason { get; set; }

    public string Status { get; set; } = "Pending";
    public int? ApprovedByUserID { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }
}