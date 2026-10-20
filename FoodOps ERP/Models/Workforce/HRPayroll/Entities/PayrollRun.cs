using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public static class PayrollRunStatus
{
    public const string Draft = "Draft";
    public const string Calculated = "Calculated";
    public const string UnderReview = "UnderReview";
    public const string Approved = "Approved";
    public const string Paid = "Paid";
    public const string Cancelled = "Cancelled";
    public static readonly string[] All = { Draft, Calculated, UnderReview, Approved, Paid, Cancelled };
}

public class PayrollRun : AuditedEntity
{
    public int PayrollRunID { get; set; }

    [Required, MaxLength(30)]
    public string RunNo { get; set; } = string.Empty;

    public int PlantID { get; set; }
    public int PeriodMonth { get; set; }
    public int PeriodYear { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = PayrollRunStatus.Draft;

    public int TotalEmployees { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalBasicPay { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAllowances { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalOvertimePay { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalDeductions { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetPayable { get; set; }

    public DateTime? CalculatedAt { get; set; }

    // Navigation
    public Plant Plant { get; set; } = null!;
    public ICollection<PayrollLine> Lines { get; set; } = new List<PayrollLine>();
    public ICollection<PayrollApproval> Approvals { get; set; } = new List<PayrollApproval>();
}