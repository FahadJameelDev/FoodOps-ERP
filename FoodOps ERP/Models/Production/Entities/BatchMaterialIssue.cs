namespace FoodOpsERP.Models.Entities.Manufacturing;

public static class MaterialIssueType
{
    public const string Issue = "Issue";
    public const string Return = "Return";
    public static readonly string[] All = { Issue, Return };
}

public class BatchMaterialIssue
{
    public int IssueID { get; set; }
    public int ProductionOrderID { get; set; }
    public long BatchID { get; set; }
    public int? RecipeItemID { get; set; }
    public string IssueType { get; set; } = MaterialIssueType.Issue;
    public decimal IssuedQty { get; set; }
    public int UOMID { get; set; }
    public long? InventoryTransactionID { get; set; }
    public int IssuedByUserID { get; set; }
    public DateTime IssuedAt { get; set; }
}