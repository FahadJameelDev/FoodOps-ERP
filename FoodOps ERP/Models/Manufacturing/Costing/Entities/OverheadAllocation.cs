namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class AllocationStatuses
{
    public const string Draft = "Draft";
    public const string Posted = "Posted";
    public static readonly string[] All = { Draft, Posted };
}

public class OverheadAllocation : AuditedEntity
{
    public int AllocationID { get; set; }
    public int PlantID { get; set; }
    public int ComponentID { get; set; }
    public DateTime PeriodMonth { get; set; }   // Always 1st of month
    public decimal AllocatedAmount { get; set; }
    public decimal BasisQty { get; set; }
    public decimal RatePerBasis { get; private set; }   // Computed
    public string Status { get; set; } = AllocationStatuses.Draft;
    public int? PostedByUserID { get; set; }
    public DateTime? PostedAt { get; set; }
}