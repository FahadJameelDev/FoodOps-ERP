using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class LeaveTypeViewModel
{
    public int LeaveTypeID { get; set; }

    [Required(ErrorMessage = "Code is required")]
    [StringLength(20)]
    public string LeaveTypeCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50)]
    public string LeaveTypeName { get; set; } = string.Empty;

    public bool IsPaid { get; set; } = true;
    public int? MaxDaysPerYear { get; set; }
    public bool CarryForward { get; set; } = false;
    public bool IsActive { get; set; } = true;
}