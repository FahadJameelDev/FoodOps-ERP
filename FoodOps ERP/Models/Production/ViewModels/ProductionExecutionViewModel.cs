using Microsoft.AspNetCore.Mvc.Rendering; // ✅ Yeh add karein

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

    // ✅ NAYA: Dropdowns ke liye strongly-typed lists
    public IEnumerable<SelectListItem>? BatchList { get; set; }
    public IEnumerable<SelectListItem>? RecipeItemList { get; set; }
    public IEnumerable<SelectListItem>? PackagingConfigList { get; set; }
}