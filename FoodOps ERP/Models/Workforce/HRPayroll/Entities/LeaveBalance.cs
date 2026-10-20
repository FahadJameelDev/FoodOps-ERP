using System.ComponentModel.DataAnnotations.Schema;
using FoodOpsERP.Models.Entities.Manufacturing;

namespace FoodOpsERP.Models.Workforce.HRPayroll.Entities;

public class LeaveBalance : VersionedEntity
{
    public int BalanceID { get; set; }
    public int EmployeeID { get; set; }
    public int LeaveTypeID { get; set; }
    public int Year { get; set; }

    [Column(TypeName = "decimal(5,1)")]
    public decimal EntitledDays { get; set; }

    [Column(TypeName = "decimal(5,1)")]
    public decimal UsedDays { get; set; }

    // FIX: Computed column
    [Column(TypeName = "decimal(5,1)")]
    public decimal RemainingDays { get; private set; }

    public Employee Employee { get; set; } = null!;
    public LeaveType LeaveType { get; set; } = null!;
}