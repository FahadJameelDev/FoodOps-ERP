using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class AttendanceViewModel
{
    public long AttendanceID { get; set; }

    [Required(ErrorMessage = "Employee is required")]
    public int EmployeeID { get; set; }
    public string? EmployeeName { get; set; }

    public int? ShiftID { get; set; }
    public string? ShiftName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime LogDate { get; set; } = DateTime.Today;

    [DataType(DataType.DateTime)]
    public DateTime? CheckInTime { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? CheckOutTime { get; set; }

    [Required]
    public string Status { get; set; } = "Present";

    public int? WorkedMinutes { get; set; }
    public string? Remarks { get; set; }

    // ✅ Dropdowns ke liye nayi properties
    public IEnumerable<SelectListItem>? EmployeeList { get; set; }
    public IEnumerable<SelectListItem>? ShiftList { get; set; }
}