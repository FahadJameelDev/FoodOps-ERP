namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class WorkCenterType
{
    public const string Mixing = "Mixing";
    public const string Cooking = "Cooking";
    public const string Filling = "Filling";
    public const string Packaging = "Packaging";
    public const string Sterilization = "Sterilization";
    public const string Storage = "Storage";
    public const string Other = "Other";
    public static readonly string[] All = { Mixing, Cooking, Filling, Packaging, Sterilization, Storage, Other };
}

public class WorkCenter : AuditedEntity
{
    public int WorkCenterID { get; set; }
    public int PlantID { get; set; }
    public string WorkCenterCode { get; set; } = string.Empty;
    public string WorkCenterName { get; set; } = string.Empty;
    public string CenterType { get; set; } = WorkCenterType.Other;
    public decimal? CapacityPerHour { get; set; }
    public int? CapacityUOMID { get; set; }
    public bool IsActive { get; set; } = true;
}