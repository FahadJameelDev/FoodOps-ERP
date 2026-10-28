using FoodOpsERP.Models.ViewModels.Inventory;
using FoodOpsERP.Services.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FoodOpsERP.Controllers.SupplyChain.Inventory;

[Authorize(Roles = "Administrator, Store Manager")] // <-- YEH LINE NAYI HAI

public class PurchaseOrdersController : BaseInventoryController
{
    private readonly IPurchaseOrderService _svc;
    private readonly IInventoryLookupService _lookups;

    public PurchaseOrdersController(IPurchaseOrderService svc, IInventoryLookupService lookups)
    { _svc = svc; _lookups = lookups; }

    [HttpGet]
    public async Task<IActionResult> Index() => View(await _svc.GetAllAsync());

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var model = await _svc.GetByIdAsync(id);
        return model is null ? NotFound() : View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadLookupsAsync();
        return FormView("_PurchaseOrderForm", "Create", new PurchaseOrderViewModel { OrderDate = DateTime.Today, Status = "Draft" });
    }

    [HttpPost]
    public async Task<IActionResult> Create(PurchaseOrderViewModel model)
    {
        RequireItems(model);
        if (ModelState.IsValid)
        {
            var error = await TryAsync(() => _svc.CreateAsync(model));
            if (error is null) return Saved("Purchase order created successfully");
            ModelState.AddModelError("", error);
        }
        await LoadLookupsAsync();
        return Invalid("_PurchaseOrderForm", "Create", model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _svc.GetByIdAsync(id);
        if (model is null) return NotFound();
        await LoadLookupsAsync();
        return FormView("_PurchaseOrderForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, PurchaseOrderViewModel model)
    {
        if (id != model.POID) return BadRequest();
        RequireItems(model);
        if (ModelState.IsValid)
        {
            var error = await TryAsync(() => _svc.UpdateAsync(model));
            if (error is null) return Saved("Purchase order updated successfully");
            ModelState.AddModelError("", error);
        }
        await LoadLookupsAsync();
        return Invalid("_PurchaseOrderForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id)
    {
        var error = await TryAsync(() => _svc.ApproveAsync(id));
        return error is null ? Saved("Purchase order approved") : Failed(error);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var error = await TryAsync(() => _svc.DeleteAsync(id));
        return error is null ? Saved("Purchase order deleted") : Failed(error);
    }

    private void RequireItems(PurchaseOrderViewModel m)
    {
        if (m.Items is null || m.Items.Count == 0) ModelState.AddModelError("", "Add at least one item.");
    }

    private async Task LoadLookupsAsync()
    {
        ViewData["Suppliers"] = await _lookups.SuppliersAsync();
        ViewData["Plants"] = await _lookups.PlantsAsync();
        ViewData["Products"] = await _lookups.ProductsAsync();
        ViewData["Uoms"] = await _lookups.UomsAsync();
    }
}