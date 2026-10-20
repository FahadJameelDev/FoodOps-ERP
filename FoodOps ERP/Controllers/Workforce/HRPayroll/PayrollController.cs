using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using FoodOpsERP.Services.Workforce.HRPayroll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Workforce.HRPayroll;

public class PayrollController : AppController
{
    private readonly IPayrollService _service;
    private readonly ApplicationDbContext _db;

    public PayrollController(IPayrollService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    // 1. Payroll Processing (Default)
    public async Task<IActionResult> Index()
    {
        var list = await _service.GetAllAsync();
        return View(list);
    }

    // 2. Payroll Review (Sirf Calculated dikhana hai)
    public async Task<IActionResult> Review()
    {
        var all = await _service.GetAllAsync();
        var list = all.Where(x => x.Status == "Calculated" || x.Status == "UnderReview").ToList();
        return View(list);
    }

    // 3. Payroll Finalization (Sirf Approved dikhana hai)
    public async Task<IActionResult> Finalize()
    {
        var all = await _service.GetAllAsync();
        var list = all.Where(x => x.Status == "Approved").ToList();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadPlantsAsync();
        return FormView("_PayrollForm", "Create", new { });
    }

    [HttpPost]
    public async Task<IActionResult> Create(int plantId, int month, int year)
    {
        var result = await _service.CreateRunAsync(plantId, month, year, GetCurrentUserId());
        if (!result.Success) return Failed(result.Message);
        return Saved(result.Message, "Index");
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Calculate(int id)
    {
        var result = await _service.CalculatePayrollAsync(id, GetCurrentUserId());
        if (!result.Success) return Failed(result.Message);
        return Saved(result.Message, "Details");
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id, string comments)
    {
        var result = await _service.ApprovePayrollAsync(id, GetCurrentUserId(), comments);
        if (!result.Success) return Failed(result.Message);
        return Saved(result.Message, "Details");
    }

    [HttpPost]
    public async Task<IActionResult> FinalizePayroll(int id) // Method ka naam badal diya taake action conflict na ho
    {
        var result = await _service.FinalizePayrollAsync(id, GetCurrentUserId());
        if (!result.Success) return Failed(result.Message);
        return Saved(result.Message, "Details");
    }

    private async Task LoadPlantsAsync()
    {
        ViewBag.Plants = new SelectList(await _db.Plants.AsNoTracking().OrderBy(p => p.PlantName).ToListAsync(), "PlantID", "PlantName");
    }
}