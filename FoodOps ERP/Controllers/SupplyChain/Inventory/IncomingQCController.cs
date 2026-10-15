using FoodOpsERP.Models.ViewModels.Inventory;
using FoodOpsERP.Services.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace FoodOpsERP.Controllers.SupplyChain.Inventory;

public class IncomingQCController : BaseInventoryController
{
    private readonly IIncomingQCService _svc;
    private readonly IInventoryLookupService _lookups;

    public IncomingQCController(IIncomingQCService svc, IInventoryLookupService lookups)
    { _svc = svc; _lookups = lookups; }

    [HttpGet]
    public async Task<IActionResult> Index() => View(await _svc.GetPendingQCAsync());

    [HttpGet]
    public async Task<IActionResult> Inspect(long id)    // id = BatchID
    {
        var b = await _lookups.GetBatchAsync(id);
        if (b is null) return NotFound();
        return FormView("_QCForm", "Inspect", new QCDecisionViewModel
        { Id = id, BatchNumber = b.BatchNumber, ProductName = b.ProductName, Quantity = b.CurrentQuantity });
    }

    [HttpPost]
    public async Task<IActionResult> Inspect(QCDecisionViewModel model)
    {
        if (ModelState.IsValid)
        {
            var uid = GetCurrentUserId();
            var error = await TryAsync(async () =>
            {
                switch (model.Decision)
                {
                    case "Pass": await _svc.PassQCAsync(model.Id, uid); break;
                    case "Fail": await _svc.FailQCAsync(model.Id, uid, model.Remarks!); break;
                    default: await _svc.HoldBatchAsync(model.Id, uid, model.Remarks!); break;
                }
            });
            if (error is null) return Saved($"Batch {model.BatchNumber}: QC {(model.Decision == "Pass" ? "passed" : model.Decision == "Fail" ? "failed" : "put on hold")}");
            ModelState.AddModelError("", error);
        }
        return Invalid("_QCForm", "Inspect", model);
    }

    [HttpPost]
    public async Task<IActionResult> Release(long id)
    {
        var error = await TryAsync(() => _svc.ReleaseBatchAsync(id, GetCurrentUserId()));
        return error is null ? Saved("Batch released") : Failed(error);
    }
}