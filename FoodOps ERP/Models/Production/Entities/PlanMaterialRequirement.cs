namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class SuggestedActions
{
    public const string None = "None";
    public const string Purchase = "Purchase";
    public const string Transfer = "Transfer";

    public static readonly string[] All =
    {
        None, Purchase, Transfer
    };
}

public class PlanMaterialRequirement : VersionedEntity
{
    public long RequirementID { get; set; }

    public int PlanLineID { get; set; }

    public int IngredientProductID { get; set; }

    public decimal RequiredQty { get; set; }

    public int UOMID { get; set; }

    public decimal AvailableQty { get; set; }

    public decimal ShortageQty { get; private set; }   // Computed column

    public string SuggestedAction { get; set; } = SuggestedActions.None;

    public DateTime CalculatedAt { get; set; }

    public int CalculatedByUserID { get; set; }

    // Navigation
    public ProductionPlanLine PlanLine { get; set; } = null!;
}