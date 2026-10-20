using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoodOpsERP.Models.Entities.Manufacturing;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class PayrollLine : VersionedEntity
{
    public long PayrollLineID { get; set; }
    public int PayrollRunID { get; set; }
    public int EmployeeID { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasicPay { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal HousingAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TransportAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OtherAllowance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OvertimePay { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal LeaveDeduction { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OtherDeduction { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrossPay { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalDeductions { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetPay { get; set; }

    public int WorkingDays { get; set; }
    public int PresentDays { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal OTHours { get; set; }

    [MaxLength(200)]
    public string? Remarks { get; set; }

    // Navigation
    public PayrollRun PayrollRun { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
}