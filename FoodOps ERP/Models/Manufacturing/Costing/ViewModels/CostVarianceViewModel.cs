using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.Manufacturing.Costing.ViewModels;

public class CostVarianceViewModel
{
    public int VarianceID { get; set; }

    public int ProductionOrderID { get; set; }
    public string? OrderNumber { get; set; }

    public decimal MaterialPriceVariance { get; set; }
    public decimal MaterialUsageVariance { get; set; }
    public decimal LaborVariance { get; set; }
    public decimal OverheadVariance { get; set; }
    public decimal YieldVariance { get; set; }

    public decimal TotalVarianceAmount { get; set; }
    public decimal? TotalVariancePercent { get; set; }
    public string Severity { get; set; } = "Normal";
    public string ReviewStatus { get; set; } = "NotRequired";
    public string? ReviewComments { get; set; }
    public DateTime CalculatedAt { get; set; }
}