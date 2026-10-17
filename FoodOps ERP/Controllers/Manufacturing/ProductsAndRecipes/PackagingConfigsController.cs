using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;
using FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Manufacturing.ProductsAndRecipes;

public class PackagingConfigsController : AppController
{
    private readonly IPackagingConfigService _service;
    private readonly ApplicationDbContext _db;

    public PackagingConfigsController(IPackagingConfigService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    public async Task<IActionResult> Index(int? productId = null)
    {
        var list = await _service.GetAllAsync(productId);
        ViewBag.ProductId = productId;
        // FIX: Lamba path hata diya (agar Program.cs update hai)
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? productId = null)
    {
        await LoadLookupsAsync(); // FIX: Dropdowns Load Kiye
        var model = new PackagingConfigViewModel
        {
            ProductID = productId ?? 0,
            IsActive = true
        };
        return FormView("_PackagingConfigForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PackagingConfigViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync();
            return Invalid("_PackagingConfigForm", "Create", model);
        }

        var userId = 1; // TODO
        var result = await _service.CreateAsync(model, userId);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await LoadLookupsAsync();
            return Invalid("_PackagingConfigForm", "Create", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();

        await LoadLookupsAsync(); // FIX: Dropdowns Load Kiye
        return FormView("_PackagingConfigForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PackagingConfigViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync();
            return Invalid("_PackagingConfigForm", "Edit", model);
        }

        var userId = 1; // TODO
        var result = await _service.UpdateAsync(model, userId);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await LoadLookupsAsync();
            return Invalid("_PackagingConfigForm", "Edit", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result.Success)
            return Failed(result.Message);

        return Saved(result.Message);
    }

    // Helper Method
    private async Task LoadLookupsAsync()
    {
        ViewBag.Products = new SelectList(await _db.Products.AsNoTracking().OrderBy(x => x.ProductName).ToListAsync(), "ProductID", "ProductName");
        ViewBag.UOMs = new SelectList(await _db.UnitsOfMeasure.AsNoTracking().OrderBy(x => x.UOMName).ToListAsync(), "UOMID", "UOMName");
    }
}