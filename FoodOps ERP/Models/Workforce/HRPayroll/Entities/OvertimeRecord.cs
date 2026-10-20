using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Entities.Manufacturing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class OvertimeRecord : AuditedEntity
{
    public int OvertimeID { get; set; }
    public int EmployeeID { get; set; }

    public DateTime OTDate { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal OTHours { get; set; }

    [Column(TypeName = "decimal(3,2)")]
    public decimal OTRateMultiplier { get; set; } = 1.50m;

    [MaxLength(300)]
    public string? Reason { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending / Approved / Rejected

    public int? ApprovedByUserID { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // Navigation
    public Employee Employee { get; set; } = null!;
    public User? ApprovedByUser { get; set; }
}