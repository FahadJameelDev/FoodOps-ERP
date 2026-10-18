namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class PlanStatus
{
    public const string Draft = "Draft";
    public const string Approved = "Approved";
    public const string Released = "Released";
    public const string Closed = "Closed";
    public const string Cancelled = "Cancelled";
    public static readonly string[] All = { Draft, Approved, Released, Closed, Cancelled };
}

public class ProductionPlan : AuditedEntity
{
    public int PlanID { get; set; }
    public string PlanCode { get; set; } = string.Empty;
    public int PlantID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = PlanStatus.Draft;
    public int? ApprovedByUserID { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? Notes { get; set; }

    public ICollection<ProductionPlanLine> Lines { get; set; } = new List<ProductionPlanLine>();
}