namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class CostSourceTypes
{
    public const string MaterialIssue = "MaterialIssue";
    public const string Labor = "Labor";
    public const string Overhead = "Overhead";
    public const string Wastage = "Wastage";
    public const string Manual = "Manual";
    public static readonly string[] All = { MaterialIssue, Labor, Overhead, Wastage, Manual };
}

public class BatchCostLine : VersionedEntity
{
    public long CostLineID { get; set; }
    public int BatchCostID { get; set; }
    public int ComponentID { get; set; }
    public string SourceType { get; set; } = CostSourceTypes.Manual;
    public long? SourceID { get; set; }
    public int? ProductID { get; set; }
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; private set; }   // Computed
    public string? Description { get; set; }

    public BatchCostSummary BatchCost { get; set; } = null!;
}