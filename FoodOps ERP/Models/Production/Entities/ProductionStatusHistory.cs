namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class StatusEntityType
{
    public const string Plan = "Plan";
    public const string Order = "Order";
    public const string Exception = "Exception";
    public static readonly string[] All = { Plan, Order, Exception };
}

public class ProductionStatusHistory
{
    public long HistoryID { get; set; }
    public string EntityType { get; set; } = StatusEntityType.Order;
    public long EntityID { get; set; }
    public string? FromStatus { get; set; }
    public string ToStatus { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public int ChangedByUserID { get; set; }
    public DateTime ChangedAt { get; set; }
}