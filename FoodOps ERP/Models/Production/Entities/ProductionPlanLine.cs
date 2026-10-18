namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class PlanLineStatus
{
    public const string Planned = "Planned";
    public const string OrderCreated = "OrderCreated";
    public const string Cancelled = "Cancelled";
    public static readonly string[] All = { Planned, OrderCreated, Cancelled };
}

public class ProductionPlanLine : VersionedEntity
{
    public int PlanLineID { get; set; }
    public int PlanID { get; set; }
    public int LineNo { get; set; }
    public int ProductID { get; set; }
    public int RecipeID { get; set; }
    public decimal PlannedQty { get; set; }
    public int UOMID { get; set; }
    public DateTime PlannedDate { get; set; }
    public int? DemandID { get; set; }
    public byte Priority { get; set; } = 3;
    public string Status { get; set; } = PlanLineStatus.Planned;

    public ProductionPlan Plan { get; set; } = null!;
    public ICollection<PlanMaterialRequirement> Requirements { get; set; } = new List<PlanMaterialRequirement>();
}