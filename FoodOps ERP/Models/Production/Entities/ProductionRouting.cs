namespace FoodOpsERP.Models.Entities.Manufacturing;

public class ProductionRouting : AuditedEntity
{
    public int RoutingID { get; set; }
    public int ProductID { get; set; }
    public int WorkCenterID { get; set; }
    public int SequenceOrder { get; set; }
    public string OperationName { get; set; } = string.Empty;
    public decimal StandardMinutes { get; set; }
    public decimal SetupMinutes { get; set; }
    public bool IsActive { get; set; } = true;
}