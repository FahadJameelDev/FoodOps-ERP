using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

public class PayrollViewModel
{
    public int PayrollRunID { get; set; }
    public string RunNo { get; set; } = string.Empty;
    public int PlantID { get; set; }
    public string? PlantName { get; set; }
    public int PeriodMonth { get; set; }
    public int PeriodYear { get; set; }
    public string Status { get; set; } = "Draft";

    public int TotalEmployees { get; set; }
    public decimal TotalBasicPay { get; set; }
    public decimal TotalAllowances { get; set; }
    public decimal TotalOvertimePay { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetPayable { get; set; }
    public DateTime? CalculatedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<PayrollLineViewModel> Lines { get; set; } = new();
}

public class PayrollLineViewModel
{
    public long PayrollLineID { get; set; }
    public int PayrollRunID { get; set; }
    public int EmployeeID { get; set; }
    public string? EmployeeName { get; set; }

    public decimal BasicPay { get; set; }
    public decimal HousingAllowance { get; set; }
    public decimal TransportAllowance { get; set; }
    public decimal OtherAllowance { get; set; }
    public decimal OvertimePay { get; set; }

    public decimal LeaveDeduction { get; set; }
    public decimal OtherDeduction { get; set; }

    public decimal GrossPay { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetPay { get; set; }

    public int WorkingDays { get; set; }
    public int PresentDays { get; set; }
    public decimal OTHours { get; set; }
    public string? Remarks { get; set; }
}