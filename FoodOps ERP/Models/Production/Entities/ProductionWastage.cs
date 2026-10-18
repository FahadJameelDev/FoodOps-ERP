namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class WastageTypes
{
    public const string ProcessLoss = "ProcessLoss";
    public const string Spillage = "Spillage";
    public const string Rework = "Rework";
    public const string Reject = "Reject";
    public const string Expired = "Expired";
    public const string Other = "Other";

    public static readonly string[] All =
    {
        ProcessLoss, Spillage, Rework, Reject, Expired, Other
    };
}

public class ProductionWastage : AuditedEntity
{
    public int WastageID { get; set; }

    public int ProductionOrderID { get; set; }

    public int ProductID { get; set; }

    public long? BatchID { get; set; }

    public int? WorkCenterID { get; set; }

    public string WastageType { get; set; } = WastageTypes.Other;

    public decimal Quantity { get; set; }

    public int UOMID { get; set; }

    public string? Stage { get; set; }

    public string Reason { get; set; } = string.Empty;

    public int RecordedByUserID { get; set; }

    public DateTime RecordedAt { get; set; }
}