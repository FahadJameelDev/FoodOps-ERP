using FoodOpsERP.Services.Manufacturing.Production;
using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.Production.ViewModels;
using FoodOpsERP.Services.Manufacturing.Production;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Manufacturing.Production;

public class DemandsController : AppController
{
    private readonly IDemandService _service;
    private readonly ApplicationDbContext _db;

    public DemandsController(IDemandService service, ApplicationDbContext db)
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

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadLookupsAsync();
        var model = new DemandViewModel
        {
            RequiredDate = DateTime.Today.AddDays(7),
            Priority = 3,
            Source = "Manual",
            Status = "Open"
        };
        return FormView("_DemandForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(DemandViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync();
            return Invalid("_DemandForm", "Create", model);
        }

        var userId = 1; // TODO
        var result = await _service.CreateAsync(model, userId);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await LoadLookupsAsync();
            return Invalid("_DemandForm", "Create", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();
        if (model.Status != "Open") return Failed("Only Open demands can be edited.");

        await LoadLookupsAsync();
        return FormView("_DemandForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(DemandViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync();
            return Invalid("_DemandForm", "Edit", model);
        }

        var userId = 1; // TODO
        var result = await _service.UpdateAsync(model, userId);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await LoadLookupsAsync();
            return Invalid("_DemandForm", "Edit", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = 1; // TODO
        var result = await _service.CancelAsync(id, userId);

        if (!result.Success)
            return Failed(result.Message);

        return Saved(result.Message);
    }

    private async Task LoadLookupsAsync()
    {
        ViewBag.Plants = new SelectList(await _db.Plants.AsNoTracking().OrderBy(p => p.PlantName).ToListAsync(), "PlantID", "PlantName");
        ViewBag.Products = new SelectList(await _db.Products.AsNoTracking().OrderBy(p => p.ProductName).ToListAsync(), "ProductID", "ProductName");
        ViewBag.UOMs = new SelectList(await _db.UnitsOfMeasure.AsNoTracking().OrderBy(u => u.UOMName).ToListAsync(), "UOMID", "UOMName");
        ViewBag.Sources = new SelectList(new[] { "Manual", "Forecast", "CustomerOrder", "Replenishment" });
    }
}