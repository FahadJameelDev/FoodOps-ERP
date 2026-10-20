using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using FoodOpsERP.Services.Workforce.HRPayroll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Workforce.HRPayroll;

public class OvertimeController : AppController
{
    private readonly IOvertimeService _service;
    private readonly ApplicationDbContext _db;

    public OvertimeController(IOvertimeService service, ApplicationDbContext db)
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
        return FormView("_OvertimeForm", "Create", new OvertimeViewModel { OTDate = DateTime.Today, OTRateMultiplier = 1.50m });
    }

    [HttpPost]
    public async Task<IActionResult> Create(OvertimeViewModel model)
    {
        if (!ModelState.IsValid) { await LoadLookupsAsync(); return Invalid("_OvertimeForm", "Create", model); }

        var result = await _service.CreateAsync(model, GetCurrentUserId());
        if (!result.Success) { ModelState.AddModelError("", result.Message); await LoadLookupsAsync(); return Invalid("_OvertimeForm", "Create", model); }

        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _service.ApproveAsync(id, GetCurrentUserId(), true);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id)
    {
        var result = await _service.ApproveAsync(id, GetCurrentUserId(), false);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    private async Task LoadLookupsAsync()
    {
        ViewBag.Employees = new SelectList(await _db.Employees.AsNoTracking().OrderBy(e => e.FirstName).ToListAsync(), "EmployeeID", "FirstName");
    }
}