using FoodOpsERP.Models.ViewModels.Inventory;
using FoodOpsERP.Services.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace FoodOpsERP.Controllers.SupplyChain.Inventory;

public class StockController : BaseInventoryController
{
    private readonly IStockService _stock;
    private readonly IInventoryLookupService _lookups;

    public StockController(IStockService stock, IInventoryLookupService lookups)
    { _stock = stock; _lookups = lookups; }

    [HttpGet]
    public async Task<IActionResult> Balance() => View(await _stock.GetStockBalanceAsync());

    [HttpGet]
    public async Task<IActionResult> Movements() => View(await _lookups.MovementsAsync());

    [HttpGet]
    public async Task<IActionResult> Adjust(long? batchId)
    {
        await LoadBatchesAsync();
        return FormView("_StockAdjustForm", "Adjust", new StockAdjustViewModel { BatchId = batchId });
    }

    [HttpPost]
    public async Task<IActionResult> Adjust(StockAdjustViewModel model)
    {
        if (ModelState.IsValid)
        {
            var error = await TryAsync(() =>
                _stock.AdjustStockAsync(model.BatchId!.Value, model.Qty!.Value, model.Reason!, GetCurrentUserId(), model.Remarks));
            if (error is null) return Saved("Stock adjusted successfully");
            ModelState.AddModelError("", error);
        }
        await LoadBatchesAsync();
        return Invalid("_StockAdjustForm", "Adjust", model);
    }

    private async Task LoadBatchesAsync() => ViewData["Batches"] = await _lookups.BatchOptionsAsync(inStockOnly: false);
}