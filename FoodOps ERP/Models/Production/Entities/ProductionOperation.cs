namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class OperationStatus
{
    public const string Pending = "Pending";
    public const string Running = "Running";
    public const string Paused = "Paused";
    public const string Completed = "Completed";
    public const string Skipped = "Skipped";
    public static readonly string[] All = { Pending, Running, Paused, Completed, Skipped };
}

public class ProductionOperation : AuditedEntity
{
    public int OperationID { get; set; }
    public int ProductionOrderID { get; set; }
    public int? RoutingID { get; set; }
    public int WorkCenterID { get; set; }
    public int SequenceOrder { get; set; }
    public string OperationName { get; set; } = string.Empty;
    public decimal StandardMinutes { get; set; }
    public string Status { get; set; } = OperationStatus.Pending;
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? ActualMinutes { get; private set; }   // Computed
    public int? OperatorUserID { get; set; }
}