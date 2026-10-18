namespace FoodOpsERP.Models.Manufacturing.Production.ViewModels;

public class ProductionExecutionViewModel
{
    public ProductionOrderViewModel Order { get; set; } = new();
    public List<ProductionOperationViewModel> Operations { get; set; } = new();

    // Material Issue / Output Forms ke liye
    public long? SelectedBatchID { get; set; }
    public decimal IssueQty { get; set; }
    public int? RecipeItemID { get; set; }
    public decimal ProducedQty { get; set; }
    public decimal RejectedQty { get; set; }
    public int? PackagingConfigID { get; set; }
}