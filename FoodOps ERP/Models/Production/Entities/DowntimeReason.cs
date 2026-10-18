namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class DowntimeCategory
{
    public const string Planned = "Planned";
    public const string Unplanned = "Unplanned";
    public static readonly string[] All = { Planned, Unplanned };
}

public static class DowntimeSubCategory
{
    public const string Breakdown = "Breakdown";
    public const string Changeover = "Changeover";
    public const string Cleaning = "Cleaning";
    public const string Material = "Material";
    public const string Utility = "Utility";
    public const string Quality = "Quality";
    public static readonly string[] All = { Breakdown, Changeover, Cleaning, Material, Utility, Quality };
}

public class DowntimeReason : AuditedEntity
{
    public int ReasonID { get; set; }
    public string ReasonCode { get; set; } = string.Empty;
    public string ReasonName { get; set; } = string.Empty;
    public string Category { get; set; } = DowntimeCategory.Unplanned;
    public string SubCategory { get; set; } = DowntimeSubCategory.Breakdown;
    public bool IsActive { get; set; } = true;
}