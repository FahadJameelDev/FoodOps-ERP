namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class OrderStatus
{
    public const string Planned = "Planned";
    public const string Released = "Released";
    public const string InProgress = "InProgress";
    public const string OnHold = "OnHold";
    public const string Completed = "Completed";
    public const string Closed = "Closed";
    public const string Cancelled = "Cancelled";
    public static readonly string[] All = { Planned, Released, InProgress, OnHold, Completed, Closed, Cancelled };
}

public class ProductionOrder : AuditedEntity
{
    public int ProductionOrderID { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public int PlantID { get; set; }
    public int? PlanLineID { get; set; }
    public int ProductID { get; set; }
    public int RecipeID { get; set; }
    public decimal TargetQty { get; set; }
    public decimal ProducedQty { get; set; }
    public decimal RejectedQty { get; set; }
    public int UOMID { get; set; }
    public byte Priority { get; set; } = 3;
    public DateTime PlannedStart { get; set; }
    public DateTime PlannedEnd { get; set; }
    public DateTime? ActualStart { get; set; }
    public DateTime? ActualEnd { get; set; }
    public string Status { get; set; } = OrderStatus.Planned;
    public string? Remarks { get; set; }
}