using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Executive.ViewModels;
using FoodOpsERP.Services.Executive;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // <-- YEH ADD KAREIN

namespace FoodOpsERP.Controllers.Executive;

[Authorize(Roles = "Administrator, Executive")] // <-- YEH ADD KAREIN

public class ReportsController : AppController
{
    private readonly IReportService _service;
    private readonly ApplicationDbContext _db;

    public ReportsController(IReportService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var model = new ReportFilterViewModel();
        await PopulateDropdownsAsync(model); // ✅ Naya method
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Generate(ReportFilterViewModel filter)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(filter);
            return View("Index", filter);
        }

        var result = await _service.GenerateReportAsync(filter);
        return View("Result", result);
    }

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
        return View(result);
    }

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
        return View(result);
    }

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
        return View(result);
    }

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
        return View(result);
    }

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
        return View(result);
    }

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
        return View(result);
    }

    // ✅ Naya Method
    private async Task PopulateDropdownsAsync(ReportFilterViewModel model)
    {
        model.PlantList = await _db.Plants.AsNoTracking()
            .OrderBy(p => p.PlantName)
            .Select(p => new SelectListItem { Value = p.PlantID.ToString(), Text = p.PlantName })
            .ToListAsync();
    }
}