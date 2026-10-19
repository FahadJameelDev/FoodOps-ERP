namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class BatchCostStatuses
{
    public const string Provisional = "Provisional";
    public const string Final = "Final";
    public static readonly string[] All = { Provisional, Final };
}

public class BatchCostSummary : AuditedEntity
{
    public int BatchCostID { get; set; }
    public int ProductionOrderID { get; set; }   // 1:1 UNIQUE
    public decimal ActualMaterialCost { get; set; }
    public decimal ActualLaborCost { get; set; }
    public decimal ActualOverheadCost { get; set; }
    public decimal TotalBatchCost { get; private set; }   // Computed
    public decimal GoodQty { get; set; }
    public decimal? UnitCost { get; private set; }        // Computed
    public decimal? StandardUnitCostSnapshot { get; set; }
    public string Status { get; set; } = BatchCostStatuses.Provisional;
    public DateTime CalculatedAt { get; set; }
    public int? FinalizedByUserID { get; set; }
    public DateTime? FinalizedAt { get; set; }

    public ICollection<BatchCostLine> Lines { get; set; } = new List<BatchCostLine>();
}