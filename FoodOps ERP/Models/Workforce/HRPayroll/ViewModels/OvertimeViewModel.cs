using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class OvertimeViewModel
{
    public int OvertimeID { get; set; }

    [Required(ErrorMessage = "Employee is required")]
    public int EmployeeID { get; set; }
    public string? EmployeeName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime OTDate { get; set; } = DateTime.Today;

    [Required]
    [Range(0.01, 24, ErrorMessage = "OT Hours must be between 0.01 and 24")]
    public decimal OTHours { get; set; }

    [Required]
    [Range(1, 5)]
    public decimal OTRateMultiplier { get; set; } = 1.50m;

    [StringLength(300)]
    public string? Reason { get; set; }

    public string Status { get; set; } = "Pending";
    public int? ApprovedByUserID { get; set; }
    public DateTime? ApprovedAt { get; set; }
}