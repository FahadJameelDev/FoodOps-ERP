using FoodOpsERP.Infrastructure;
using FoodOpsERP.Services.Executive;
using Microsoft.AspNetCore.Mvc;

namespace FoodOpsERP.Controllers.Executive;

public class DashboardController : AppController
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service) => _service = service;

    // 1. Main Dashboard
    public async Task<IActionResult> Index()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    // 2. Production KPIs Page
    public async Task<IActionResult> ProductionKpis()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    // 3. Inventory KPIs Page
    public async Task<IActionResult> InventoryKpis()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    // 4. Quality KPIs Page
    public async Task<IActionResult> QualityKpis()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    // 5. Cost & Variance KPIs Page
    public async Task<IActionResult> CostKpis()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    // 6. Workforce KPIs Page
    public async Task<IActionResult> WorkforceKpis()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    // 7. Alerts Page
    public async Task<IActionResult> Alerts()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    // 8. Action Queue Page
    public async Task<IActionResult> ActionQueue()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }

    // 9. Trends Page
    public async Task<IActionResult> Trends()
    {
        var model = await _service.GetDashboardDataAsync();
        return View(model);
    }
}