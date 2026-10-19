namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class VarianceTypes
{
    public const string MaterialPrice = "MaterialPrice";
    public const string MaterialUsage = "MaterialUsage";
    public const string Labor = "Labor";
    public const string Overhead = "Overhead";
    public const string Yield = "Yield";
    public const string Total = "Total";
    public static readonly string[] All = { MaterialPrice, MaterialUsage, Labor, Overhead, Yield, Total };
}

public class VarianceThreshold : AuditedEntity
{
    public int ThresholdID { get; set; }
    public int? PlantID { get; set; }      // NULL = all plants
    public int? ProductID { get; set; }    // NULL = all products
    public string VarianceType { get; set; } = VarianceTypes.Total;
    public decimal WarningPercent { get; set; }
    public decimal CriticalPercent { get; set; }
    public bool IsActive { get; set; } = true;
}