namespace FoodOpsERP.Models.Entities.Manufacturing;

public class BatchOutputLog
{
    public int OutputID { get; set; }
    public int ProductionOrderID { get; set; }
    public long OutputBatchID { get; set; }
    public decimal ProducedQty { get; set; }
    public decimal RejectedQty { get; set; }
    public int UOMID { get; set; }
    public int? PackagingConfigID { get; set; }
    public int RecordedByUserID { get; set; }
    public DateTime RecordedAt { get; set; }
    public string? Remarks { get; set; }
}