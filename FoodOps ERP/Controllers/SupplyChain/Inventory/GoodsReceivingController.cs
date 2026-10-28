using FoodOpsERP.Models.ViewModels.Inventory;
using FoodOpsERP.Services.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // <-- YEH LINE NAYI HAI


namespace FoodOpsERP.Controllers.SupplyChain.Inventory;

[Authorize(Roles = "Administrator, Store Manager")] // <-- YEH LINE NAYI HAI

public class GoodsReceivingController : BaseInventoryController
{
    private readonly IGoodsReceivingService _svc;
    private readonly IInventoryLookupService _lookups;

    public GoodsReceivingController(IGoodsReceivingService svc, IInventoryLookupService lookups)
    { _svc = svc; _lookups = lookups; }

    [HttpGet]
    public async Task<IActionResult> Index() => View(await _svc.GetPendingPOsAsync());

    [HttpGet]
    public async Task<IActionResult> Create(int id)      // id = POID
    {
        var model = await _svc.GetPOForReceivingAsync(id);
        if (model is null) return NotFound();
        await LoadLookupsAsync();
        return FormView("_GRNForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GRNViewModel model)
    {
        if (ModelState.IsValid)
        {
            var created = false;
            var error = await TryAsync(async () => created = await _svc.CreateGRNAsync(model, GetCurrentUserId()));
            if (error is null && created) return Saved("Goods received successfully");
            ModelState.AddModelError("", error ?? "Could not create the GRN.");
        }
        await LoadLookupsAsync();
        return Invalid("_GRNForm", "Create", model);
    }

    private async Task LoadLookupsAsync() => ViewData["Bins"] = await _lookups.BinsAsync();
}