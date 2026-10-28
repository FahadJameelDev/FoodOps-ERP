using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;
using FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace FoodOpsERP.Controllers.Manufacturing.ProductsAndRecipes;

[Authorize(Roles = "Administrator, Production Manager")]

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
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new ProductViewModel();
        await PopulateDropdowns(model);
        return FormView("_ProductForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model);
            return Invalid("_ProductForm", "Create", model);
        }

        var result = await _service.CreateAsync(model);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdowns(model);
            return Invalid("_ProductForm", "Create", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();

        await PopulateDropdowns(model);
        return FormView("_ProductForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(model);
            return Invalid("_ProductForm", "Edit", model);
        }

        var result = await _service.UpdateAsync(model);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdowns(model);
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

    // ✅ ViewData ki jagah ab Model mein data ja raha hai
    private async Task PopulateDropdowns(ProductViewModel model)
    {
        model.CategoryList = await _db.Categories.AsNoTracking()
            .OrderBy(x => x.CategoryName)
            .Select(x => new SelectListItem { Value = x.CategoryID.ToString(), Text = x.CategoryName })
            .ToListAsync();

        model.UOMList = await _db.UnitsOfMeasure.AsNoTracking()
            .OrderBy(x => x.UOMName)
            .Select(x => new SelectListItem { Value = x.UOMID.ToString(), Text = x.UOMName })
            .ToListAsync();
    }
}