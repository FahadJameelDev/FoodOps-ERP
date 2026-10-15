using FoodOpsERP.Models.ViewModels.Inventory;
using FoodOpsERP.Services.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace FoodOpsERP.Controllers.SupplyChain.Inventory;

public class MaterialIssueController : BaseInventoryController
{
    private readonly IStockService _stock;
    private readonly IInventoryLookupService _lookups;

    public MaterialIssueController(IStockService stock, IInventoryLookupService lookups)
    { _stock = stock; _lookups = lookups; }

    [HttpGet]
    public async Task<IActionResult> Index() => View(await _stock.GetLotsAsync());

    [HttpGet] public Task<IActionResult> Create(long? batchId) => FormAsync("Issue", "Create", batchId);
    [HttpGet] public Task<IActionResult> Return(long? batchId) => FormAsync("Return", "Return", batchId);

    [HttpPost] public Task<IActionResult> Create(StockMovementViewModel model) => SaveAsync(model, "Issue", "Create");
    [HttpPost] public Task<IActionResult> Return(StockMovementViewModel model) => SaveAsync(model, "Return", "Return");

    private async Task<IActionResult> FormAsync(string kind, string page, long? batchId)
    {
        await LoadBatchesAsync(kind);
        return FormView("_MaterialIssueForm", page, new StockMovementViewModel { Kind = kind, BatchId = batchId });
    }

    private async Task<IActionResult> SaveAsync(StockMovementViewModel model, string kind, string page)
    {
        model.Kind = kind;
        if (ModelState.IsValid)
        {
            var qty = kind == "Issue" ? -model.Qty!.Value : model.Qty!.Value;
            var error = await TryAsync(() =>
                _stock.AdjustStockAsync(model.BatchId!.Value, qty, $"Material {kind}", GetCurrentUserId(), model.Remarks));
            if (error is null) return Saved(kind == "Issue" ? "Material issued successfully" : "Material returned successfully");
            ModelState.AddModelError("", error);
        }
        await LoadBatchesAsync(kind);
        return Invalid("_MaterialIssueForm", page, model);
    }

    private async Task LoadBatchesAsync(string kind) =>
        ViewData["Batches"] = await _lookups.BatchOptionsAsync(inStockOnly: kind == "Issue");
}