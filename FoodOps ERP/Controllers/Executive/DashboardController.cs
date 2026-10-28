using FoodOpsERP.Infrastructure;
using FoodOpsERP.Services.Executive;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // <-- YEH ADD KAREIN

namespace FoodOpsERP.Controllers.Executive;

[Authorize(Roles = "Administrator, Executive")] // <-- YEH ADD KAREIN

public class DashboardController : AppController
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service) => _service = service;

    public async Task<IActionResult> Index()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    public async Task<IActionResult> ProductionKpis()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    public async Task<IActionResult> InventoryKpis()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    public async Task<IActionResult> QualityKpis()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    public async Task<IActionResult> CostKpis()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    public async Task<IActionResult> WorkforceKpis()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    public async Task<IActionResult> Alerts()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    public async Task<IActionResult> ActionQueue()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    public async Task<IActionResult> Trends()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }
}