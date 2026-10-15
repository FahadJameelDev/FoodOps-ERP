using FoodOpsERP.Models.ViewModels.Inventory;
using FoodOpsERP.Services.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace FoodOpsERP.Controllers.SupplyChain.Inventory;

public class WastageController : BaseInventoryController
{
    private readonly IStockService _stock;
    private readonly IInventoryLookupService _lookups;

    public WastageController(IStockService stock, IInventoryLookupService lookups)
    { _stock = stock; _lookups = lookups; }

    [HttpGet]
    public async Task<IActionResult> Index() => View(await _stock.GetLotsAsync());

    [HttpGet]
    public async Task<IActionResult> Create(long? batchId)
    {
        await LoadBatchesAsync();
        return FormView("_WastageForm", "Create", new WastageViewModel { BatchId = batchId });
    }

    [HttpPost]
    public async Task<IActionResult> Create(WastageViewModel model)
    {
        if (ModelState.IsValid)
        {
            var error = await TryAsync(() =>
                _stock.AdjustStockAsync(model.BatchId!.Value, -model.Qty!.Value, $"Wastage: {model.Reason}", GetCurrentUserId(), model.Remarks));
            if (error is null) return Saved("Wastage recorded");
            ModelState.AddModelError("", error);
        }
        await LoadBatchesAsync();
        return Invalid("_WastageForm", "Create", model);
    }

    private async Task LoadBatchesAsync() => ViewData["Batches"] = await _lookups.BatchOptionsAsync(inStockOnly: true);
}