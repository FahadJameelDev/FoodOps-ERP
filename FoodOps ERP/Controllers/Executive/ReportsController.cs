using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Executive.ViewModels;
using FoodOpsERP.Services.Executive;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Executive;

public class ReportsController : AppController
{
    private readonly IReportService _service;
    private readonly ApplicationDbContext _db;

    public ReportsController(IReportService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    // 1. Reports Home Page (Filters)
    public async Task<IActionResult> Index()
    {
        await LoadPlantsAsync();
        return View(new ReportFilterViewModel());
    }

    // 2. Generate Report (Smart Result View - abhi ke liye)
    [HttpPost]
    public async Task<IActionResult> Generate(ReportFilterViewModel filter)
    {
        if (!ModelState.IsValid)
        {
            await LoadPlantsAsync();
            return View("Index", filter);
        }

        var result = await _service.GenerateReportAsync(filter);
        return View("Result", result);
    }

    // ═════════════════════════════════════════════
    // ALAG ALAG REPORTS KE LIYE 6 NAYE METHODS
    // ═════════════════════════════════════════════

    // 3. Production Report
    [HttpGet]
    public async Task<IActionResult> Production(DateTime? startDate, DateTime? endDate, int? plantId)
    {
        var filter = new ReportFilterViewModel
        {
            ReportType = "Production",
            StartDate = startDate ?? DateTime.Today.AddDays(-30),
            EndDate = endDate ?? DateTime.Today,
            PlantID = plantId
        };
        var result = await _service.GenerateReportAsync(filter);
        return View(result); // Ye Production.cshtml view ko call karega
    }

    // 4. Inventory Report
    [HttpGet]
    public async Task<IActionResult> Inventory(DateTime? startDate, DateTime? endDate, int? plantId)
    {
        var filter = new ReportFilterViewModel
        {
            ReportType = "Inventory",
            StartDate = startDate ?? DateTime.Today.AddDays(-30),
            EndDate = endDate ?? DateTime.Today,
            PlantID = plantId
        };
        var result = await _service.GenerateReportAsync(filter);
        return View(result); // Ye Inventory.cshtml view ko call karega
    }

    // 5. Quality Report
    [HttpGet]
    public async Task<IActionResult> Quality(DateTime? startDate, DateTime? endDate, int? plantId)
    {
        var filter = new ReportFilterViewModel
        {
            ReportType = "Quality",
            StartDate = startDate ?? DateTime.Today.AddDays(-30),
            EndDate = endDate ?? DateTime.Today,
            PlantID = plantId
        };
        var result = await _service.GenerateReportAsync(filter);
        return View(result); // Ye Quality.cshtml view ko call karega
    }

    // 6. Cost Report
    [HttpGet]
    public async Task<IActionResult> Cost(DateTime? startDate, DateTime? endDate, int? plantId)
    {
        var filter = new ReportFilterViewModel
        {
            ReportType = "Cost",
            StartDate = startDate ?? DateTime.Today.AddDays(-30),
            EndDate = endDate ?? DateTime.Today,
            PlantID = plantId
        };
        var result = await _service.GenerateReportAsync(filter);
        return View(result); // Ye Cost.cshtml view ko call karega
    }

    // 7. Workforce Report
    [HttpGet]
    public async Task<IActionResult> Workforce(DateTime? startDate, DateTime? endDate, int? plantId)
    {
        var filter = new ReportFilterViewModel
        {
            ReportType = "Workforce",
            StartDate = startDate ?? DateTime.Today.AddDays(-30),
            EndDate = endDate ?? DateTime.Today,
            PlantID = plantId
        };
        var result = await _service.GenerateReportAsync(filter);
        return View(result); // Ye Workforce.cshtml view ko call karega
    }

    // 8. Executive Summary Report
    [HttpGet]
    public async Task<IActionResult> Executive(DateTime? startDate, DateTime? endDate, int? plantId)
    {
        var filter = new ReportFilterViewModel
        {
            ReportType = "Executive",
            StartDate = startDate ?? DateTime.Today.AddDays(-30),
            EndDate = endDate ?? DateTime.Today,
            PlantID = plantId
        };
        var result = await _service.GenerateReportAsync(filter);
        return View(result); // Ye Executive.cshtml view ko call karega
    }

    // Lookups
    private async Task LoadPlantsAsync()
    {
        ViewBag.Plants = new SelectList(await _db.Plants.AsNoTracking().OrderBy(p => p.PlantName).ToListAsync(), "PlantID", "PlantName");
    }
}