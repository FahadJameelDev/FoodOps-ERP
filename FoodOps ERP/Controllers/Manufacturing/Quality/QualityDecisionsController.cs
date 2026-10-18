using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.Quality.ViewModels;
using FoodOpsERP.Services.Manufacturing.Quality;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Manufacturing.Quality;

public class QualityDecisionsController : AppController
{
    private readonly IQualityDecisionService _service;
    private readonly ApplicationDbContext _db;

    public QualityDecisionsController(IQualityDecisionService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    // 1. Default Index (Yeh Quality Decision tab ke liye hai)
    public async Task<IActionResult> Index(long? batchId = null)
    {
        var list = await _service.GetAllAsync(batchId);
        ViewBag.BatchId = batchId;
        return View(list);
    }

    // 2. Production QC (Alag page aur active state ke liye)
    public async Task<IActionResult> ProductionQC()
    {
        var list = await _service.GetAllAsync(null);
        return View(list);
    }

    // 3. Quality Decision (Alag page aur active state ke liye)
    public async Task<IActionResult> QualityDecisions()
    {
        var list = await _service.GetAllAsync(null);
        return View(list);
    }

    // 4. Release Control (Alag page aur active state ke liye)
    public async Task<IActionResult> ReleaseControl()
    {
        var list = await _service.GetAllAsync(null);
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create(long? batchId = null)
    {
        await LoadLookupsAsync();
        var model = new QualityDecisionViewModel
        {
            BatchID = batchId ?? 0,
            Decision = "Approve"
        };
        return FormView("_DecisionForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(QualityDecisionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync();
            return Invalid("_DecisionForm", "Create", model);
        }

        var result = await _service.CreateAsync(model, 1); // TODO: Replace 1 with Identity later

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await LoadLookupsAsync();
            return Invalid("_DecisionForm", "Create", model);
        }

        return Saved(result.Message, "Index");
    }

    private async Task LoadLookupsAsync()
    {
        ViewBag.Batches = new SelectList(
            await _db.InventoryBatches.AsNoTracking()
                .OrderByDescending(b => b.BatchID)
                .Take(200)
                .Select(b => new { b.BatchID, Text = b.BatchNumber })
                .ToListAsync(),
            "BatchID", "Text");
    }
}