using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.Production.ViewModels;
using FoodOpsERP.Services.Manufacturing.Production;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Manufacturing.Production;

public class ProductionPlansController : AppController
{
    private readonly IProductionPlanService _service;
    private readonly ApplicationDbContext _db;

    public ProductionPlansController(IProductionPlanService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    public async Task<IActionResult> Index(string? status = null)
    {
        var list = await _service.GetAllAsync(status);
        ViewBag.Status = status;
        return View(list);
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadLookupsAsync();
        var model = new ProductionPlanViewModel
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(7),
            Status = "Draft",
            Lines = new List<ProductionPlanLineViewModel>()
        };
        return FormView("_PlanForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductionPlanViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync();
            return Invalid("_PlanForm", "Create", model);
        }

        var userId = 1; // TODO
        var result = await _service.CreateAsync(model, userId);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await LoadLookupsAsync();
            return Invalid("_PlanForm", "Create", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _service.ApproveAsync(id, 1);
        return result.Success ? Saved(result.Message) : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Release(int id)
    {
        var result = await _service.ReleaseAsync(id, 1);
        return result.Success ? Saved(result.Message) : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Close(int id)
    {
        var result = await _service.CloseAsync(id, 1);
        return result.Success ? Saved(result.Message) : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _service.CancelAsync(id, 1);
        return result.Success ? Saved(result.Message) : Failed(result.Message);
    }

    private async Task LoadLookupsAsync()
    {
        ViewBag.Plants = new SelectList(await _db.Plants.AsNoTracking().OrderBy(p => p.PlantName).ToListAsync(), "PlantID", "PlantName");
        ViewBag.Products = new SelectList(await _db.Products.AsNoTracking().OrderBy(p => p.ProductName).ToListAsync(), "ProductID", "ProductName");
        ViewBag.Recipes = new SelectList(await _db.Recipes.AsNoTracking().Where(r => r.Status == "Approved").OrderBy(r => r.RecipeCode).ToListAsync(), "RecipeID", "RecipeCode");
        ViewBag.UOMs = new SelectList(await _db.UnitsOfMeasure.AsNoTracking().OrderBy(u => u.UOMName).ToListAsync(), "UOMID", "UOMName");
        ViewBag.Demands = new SelectList(await _db.Demands.AsNoTracking().Where(d => d.Status == "Open").OrderBy(d => d.DemandCode).ToListAsync(), "DemandID", "DemandCode");
    }
}