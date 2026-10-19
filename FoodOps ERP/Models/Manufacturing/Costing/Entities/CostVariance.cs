namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class VarianceSeverities
{
    public const string Normal = "Normal";
    public const string Warning = "Warning";
    public const string Critical = "Critical";
    public static readonly string[] All = { Normal, Warning, Critical };
}

public static class VarianceReviewStatuses
{
    public const string NotRequired = "NotRequired";
    public const string Pending = "Pending";
    public const string Accepted = "Accepted";
    public const string Investigate = "Investigate";
    public const string Closed = "Closed";
    public static readonly string[] All = { NotRequired, Pending, Accepted, Investigate, Closed };
}

public class CostVariance : AuditedEntity
{
    public int VarianceID { get; set; }
    public int ProductionOrderID { get; set; }   // 1:1 UNIQUE
    public decimal MaterialPriceVariance { get; set; }
    public decimal MaterialUsageVariance { get; set; }
    public decimal LaborVariance { get; set; }
    public decimal OverheadVariance { get; set; }
    public decimal YieldVariance { get; set; }
    public decimal TotalVarianceAmount { get; private set; }   // Computed
    public decimal? TotalVariancePercent { get; set; }
    public string Severity { get; set; } = VarianceSeverities.Normal;
    public string ReviewStatus { get; set; } = VarianceReviewStatuses.NotRequired;
    public int? ReviewedByUserID { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewComments { get; set; }
    public DateTime CalculatedAt { get; set; }
}