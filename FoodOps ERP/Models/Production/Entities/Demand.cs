namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class DemandSource
{
    public const string Forecast = "Forecast";
    public const string CustomerOrder = "CustomerOrder";
    public const string Replenishment = "Replenishment";
    public const string Manual = "Manual";
    public static readonly string[] All = { Forecast, CustomerOrder, Replenishment, Manual };
}

public static class DemandStatus
{
    public const string Open = "Open";
    public const string Planned = "Planned";
    public const string Fulfilled = "Fulfilled";
    public const string Cancelled = "Cancelled";
    public static readonly string[] All = { Open, Planned, Fulfilled, Cancelled };
}

public class Demand : AuditedEntity
{
    public int DemandID { get; set; }
    public string DemandCode { get; set; } = string.Empty;
    public int PlantID { get; set; }
    public int ProductID { get; set; }
    public DateTime RequiredDate { get; set; }
    public decimal Quantity { get; set; }
    public int UOMID { get; set; }
    public string Source { get; set; } = DemandSource.Manual;
    public string? ReferenceNo { get; set; }
    public byte Priority { get; set; } = 3;
    public string Status { get; set; } = DemandStatus.Open;
    public string? Notes { get; set; }
}