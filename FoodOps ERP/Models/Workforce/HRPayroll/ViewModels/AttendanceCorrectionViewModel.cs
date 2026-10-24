using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class AttendanceCorrectionViewModel
{
    public int CorrectionID { get; set; }

    [Required(ErrorMessage = "Attendance record is required")]
    public long AttendanceID { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime? LogDate { get; set; }

    public DateTime? OldCheckIn { get; set; }
    public DateTime? OldCheckOut { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? NewCheckIn { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? NewCheckOut { get; set; }

    [Required(ErrorMessage = "Reason is required")]
    [StringLength(300)]
    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";
    public int? ApprovedByUserID { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // ✅ Dropdown ke liye nayi property
    public IEnumerable<SelectListItem>? AttendanceLogList { get; set; }
}