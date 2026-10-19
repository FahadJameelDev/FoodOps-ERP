using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.Costing.ViewModels;

public class BatchCostSummaryViewModel
{
    public int BatchCostID { get; set; }

    public int ProductionOrderID { get; set; }
    public string? OrderNumber { get; set; }

    public decimal ActualMaterialCost { get; set; }
    public decimal ActualLaborCost { get; set; }
    public decimal ActualOverheadCost { get; set; }

    public decimal TotalBatchCost { get; set; }
    public decimal GoodQty { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? StandardUnitCostSnapshot { get; set; }

    public string Status { get; set; } = "Provisional";
    public DateTime CalculatedAt { get; set; }
}