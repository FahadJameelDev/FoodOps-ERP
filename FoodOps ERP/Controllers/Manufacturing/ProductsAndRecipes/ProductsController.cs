using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;
using FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Manufacturing.ProductsAndRecipes;

public class ProductsController : AppController
{
    private readonly IProductService _service;
    private readonly ApplicationDbContext _db;

    public ProductsController(IProductService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _service.GetAllAsync();
        // FIX: Lamba path hata diya
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadLookupsAsync();
        return FormView("_ProductForm", "Create", new ProductViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync();
            return Invalid("_ProductForm", "Create", model);
        }

        var result = await _service.CreateAsync(model);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await LoadLookupsAsync();
            return Invalid("_ProductForm", "Create", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();

        await LoadLookupsAsync();
        return FormView("_ProductForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync();
            return Invalid("_ProductForm", "Edit", model);
        }

        var result = await _service.UpdateAsync(model);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await LoadLookupsAsync();
            return Invalid("_ProductForm", "Edit", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result.Success ? Saved(result.Message) : Failed(result.Message);
    }

    private async Task LoadLookupsAsync()
    {
        ViewBag.Categories = new SelectList(await _db.Categories.AsNoTracking().OrderBy(x => x.CategoryName).ToListAsync(), "CategoryID", "CategoryName");
        ViewBag.UOMs = new SelectList(await _db.UnitsOfMeasure.AsNoTracking().OrderBy(x => x.UOMName).ToListAsync(), "UOMID", "UOMName");
    }
}