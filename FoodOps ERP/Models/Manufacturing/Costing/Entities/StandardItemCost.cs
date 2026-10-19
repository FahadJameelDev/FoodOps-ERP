namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class StandardCostStatuses
{
    public const string Draft = "Draft";
    public const string Approved = "Approved";
    public const string Superseded = "Superseded";
    public static readonly string[] All = { Draft, Approved, Superseded };
}

public class StandardItemCost : AuditedEntity
{
    public int CostID { get; set; }
    public int ProductID { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public decimal StandardMaterialCost { get; set; }
    public decimal StandardLaborCost { get; set; }
    public decimal StandardOverheadCost { get; set; }
    public decimal TotalStandardCost { get; private set; }   // Computed
    public int CostUOMID { get; set; }
    public string Status { get; set; } = StandardCostStatuses.Draft;
    public int? ApprovedByUserID { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? Notes { get; set; }
}