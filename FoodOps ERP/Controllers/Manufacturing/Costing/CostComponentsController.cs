using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.Costing.ViewModels;
using FoodOpsERP.Services.Manufacturing.Costing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace FoodOpsERP.Controllers.Manufacturing.Costing;

[Authorize(Roles = "Administrator, Cost Accountant")] // <-- YEH LINE NAYI HAI

public class CostComponentsController : AppController
{
    private readonly ICostSetupService _service;
    private readonly IOverheadService _overheadService;
    private readonly ApplicationDbContext _db;

    public CostComponentsController(ICostSetupService service, IOverheadService overheadService, ApplicationDbContext db)
    {
        _service = service;
        _overheadService = overheadService;
        _db = db;
    }

    // --- Components ---
    public async Task<IActionResult> Index()
    {
        var list = await _service.GetAllComponentsAsync();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        LoadLookups();
        return FormView("_ComponentForm", "Create", new CostComponentViewModel { IsActive = true, ComponentType = "Overhead", AllocationBasis = "PerBatch" });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CostComponentViewModel model)
    {
        if (!ModelState.IsValid) { LoadLookups(); return Invalid("_ComponentForm", "Create", model); }

        var result = await _service.CreateComponentAsync(model, 1);
        if (!result.Success) { ModelState.AddModelError("", result.Message); LoadLookups(); return Invalid("_ComponentForm", "Create", model); }

        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetComponentByIdAsync(id);
        if (model == null) return NotFound();

        LoadLookups();
        return FormView("_ComponentForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CostComponentViewModel model)
    {
        if (!ModelState.IsValid) { LoadLookups(); return Invalid("_ComponentForm", "Edit", model); }

        var result = await _service.UpdateComponentAsync(model, 1);
        if (!result.Success) { ModelState.AddModelError("", result.Message); LoadLookups(); return Invalid("_ComponentForm", "Edit", model); }

        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var result = await _service.ToggleComponentActiveAsync(id, 1);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    // --- Standard Costs ---
    public async Task<IActionResult> StandardCosts()
    {
        var list = await _service.GetAllStandardCostsAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> CreateStandardCost()
    {
        await LoadStandardCostLookupsAsync();
        return FormView("_StandardCostForm", "CreateStandardCost", new StandardCostViewModel { EffectiveFrom = DateTime.Today });
    }

    [HttpPost]
    public async Task<IActionResult> CreateStandardCost(StandardCostViewModel model)
    {
        if (!ModelState.IsValid) { await LoadStandardCostLookupsAsync(); return Invalid("_StandardCostForm", "CreateStandardCost", model); }

        var result = await _service.CreateStandardCostAsync(model, 1);
        if (!result.Success) { ModelState.AddModelError("", result.Message); await LoadStandardCostLookupsAsync(); return Invalid("_StandardCostForm", "CreateStandardCost", model); }

        return Saved(result.Message, "StandardCosts");
    }

    [HttpPost]
    public async Task<IActionResult> ApproveStandardCost(int id)
    {
        var result = await _service.ApproveStandardCostAsync(id, 1);
        return result.Success ? Saved(result.Message, "StandardCosts") : Failed(result.Message);
    }

    // --- Overhead Allocations ---
    public async Task<IActionResult> OverheadAllocations()
    {
        var list = await _overheadService.GetAllAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> CreateOverhead()
    {
        await LoadOverheadLookupsAsync();
        return FormView("_OverheadAllocationForm", "CreateOverhead", new OverheadAllocationViewModel { PeriodMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1) });
    }

    [HttpPost]
    public async Task<IActionResult> CreateOverhead(OverheadAllocationViewModel model)
    {
        if (!ModelState.IsValid) { await LoadOverheadLookupsAsync(); return Invalid("_OverheadAllocationForm", "CreateOverhead", model); }

        var result = await _overheadService.CreateAsync(model, 1);
        if (!result.Success) { ModelState.AddModelError("", result.Message); await LoadOverheadLookupsAsync(); return Invalid("_OverheadAllocationForm", "CreateOverhead", model); }

        return Saved(result.Message, "OverheadAllocations");
    }

    [HttpPost]
    public async Task<IActionResult> PostOverhead(int id)
    {
        var result = await _overheadService.PostAsync(id, 1);
        return result.Success ? Saved(result.Message, "OverheadAllocations") : Failed(result.Message);
    }

    // --- Lookups ---
    private void LoadLookups()
    {
        ViewBag.ComponentTypes = new SelectList(new[] { "Material", "Labor", "Overhead" });
        ViewBag.AllocationBases = new SelectList(new[] { "PerBatch", "PerKg", "PerHour", "PerUnit", "Fixed" });
    }

    private async Task LoadStandardCostLookupsAsync()
    {
        ViewBag.Products = new SelectList(await _db.Products.AsNoTracking().OrderBy(p => p.ProductName).ToListAsync(), "ProductID", "ProductName");
        ViewBag.UOMs = new SelectList(await _db.UnitsOfMeasure.AsNoTracking().OrderBy(u => u.UOMName).ToListAsync(), "UOMID", "UOMName");
    }

    private async Task LoadOverheadLookupsAsync()
    {
        ViewBag.Plants = new SelectList(await _db.Plants.AsNoTracking().OrderBy(p => p.PlantName).ToListAsync(), "PlantID", "PlantName");
        ViewBag.Components = new SelectList(await _db.CostComponents.AsNoTracking().Where(c => c.IsActive).OrderBy(c => c.ComponentName).ToListAsync(), "ComponentID", "ComponentName");
    }
}