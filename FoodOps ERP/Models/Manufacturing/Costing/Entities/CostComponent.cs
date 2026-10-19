namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class ComponentTypes
{
    public const string Material = "Material";
    public const string Labor = "Labor";
    public const string Overhead = "Overhead";
    public static readonly string[] All = { Material, Labor, Overhead };
}

public static class AllocationBases
{
    public const string PerBatch = "PerBatch";
    public const string PerKg = "PerKg";
    public const string PerHour = "PerHour";
    public const string PerUnit = "PerUnit";
    public const string Fixed = "Fixed";
    public static readonly string[] All = { PerBatch, PerKg, PerHour, PerUnit, Fixed };
}

public class CostComponent : AuditedEntity
{
    public int ComponentID { get; set; }
    public string ComponentCode { get; set; } = string.Empty;
    public string ComponentName { get; set; } = string.Empty;
    public string ComponentType { get; set; } = ComponentTypes.Overhead;
    public string AllocationBasis { get; set; } = AllocationBases.PerBatch;
    public bool IsActive { get; set; } = true;
}