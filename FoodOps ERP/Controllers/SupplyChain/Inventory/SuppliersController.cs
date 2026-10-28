using FoodOpsERP.Models.ViewModels.Inventory;
using FoodOpsERP.Services.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOpsERP.Controllers.SupplyChain.Inventory;

[Authorize(Roles = "Administrator, Store Manager")] // <-- YEH LINE NAYI HAI

public class SuppliersController : BaseInventoryController
{
    private readonly ISupplierService _svc;
    public SuppliersController(ISupplierService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> Index() => View(await _svc.GetAllAsync());

    [HttpGet]
    public IActionResult Create() =>
        FormView("_SupplierForm", "Create", new SupplierViewModel { IsActive = true });

    [HttpPost]
    public async Task<IActionResult> Create(SupplierViewModel model)
    {
        if (ModelState.IsValid)
        {
            var error = await TryAsync(() => _svc.CreateAsync(model));
            if (error is null) return Saved("Supplier created successfully");
            ModelState.AddModelError("", error);
        }
        return Invalid("_SupplierForm", "Create", model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _svc.GetByIdAsync(id);
        return model is null ? NotFound() : FormView("_SupplierForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, SupplierViewModel model)
    {
        if (id != model.SupplierID) return BadRequest();
        if (ModelState.IsValid)
        {
            var error = await TryAsync(() => _svc.UpdateAsync(model));
            if (error is null) return Saved("Supplier updated successfully");
            ModelState.AddModelError("", error);
        }
        return Invalid("_SupplierForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var error = await TryAsync(() => _svc.DeleteAsync(id));
        return error is null ? Saved("Supplier deleted successfully") : Failed(error);
    }
}