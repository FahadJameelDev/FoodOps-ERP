using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class PayrollApproval : VersionedEntity
{
    public int ApprovalID { get; set; }
    public int PayrollRunID { get; set; }
    public int StepNo { get; set; } // 1 = HR, 2 = Finance, 3 = Director
    public int ApproverUserID { get; set; }

    [Required, MaxLength(20)]
    public string Decision { get; set; } = string.Empty; // Approved / Rejected

    [MaxLength(500)]
    public string? Comments { get; set; }

    public DateTime DecidedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public PayrollRun PayrollRun { get; set; } = null!;
    public User ApproverUser { get; set; } = null!;
}