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

    public async Task<IActionResult> Index()
    {
        var list = await _service.GetAllAsync();
        return View(list);
    }

    public async Task<IActionResult> Review()
    {
        var all = await _service.GetAllAsync();
        var list = all.Where(x => x.Status == "Calculated" || x.Status == "UnderReview").ToList();
        return View(list);
    }

    public async Task<IActionResult> Finalize()
    {
        var all = await _service.GetAllAsync();
        var list = all.Where(x => x.Status == "Approved").ToList();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new PayrollViewModel();
        await PopulateDropdownsAsync(model);
        return FormView("_PayrollForm", "Create", model);
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

        // ✅ FIX: Redirect back to Details with the ID parameter
        if (IsAjax) return Saved(result.Message); // Ajax ke liye JSON
        return RedirectToAction(nameof(Details), new { id = id });
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id, string comments)
    {
        var result = await _service.ApprovePayrollAsync(id, GetCurrentUserId(), comments);
        if (!result.Success) return Failed(result.Message);

        if (IsAjax) return Saved(result.Message);
        return RedirectToAction(nameof(Details), new { id = id });
    }

    [HttpPost]
    public async Task<IActionResult> FinalizePayroll(int id)
    {
        var result = await _service.FinalizePayrollAsync(id, GetCurrentUserId());
        if (!result.Success) return Failed(result.Message);

        if (IsAjax) return Saved(result.Message);
        return RedirectToAction(nameof(Details), new { id = id });
    }

    private async Task PopulateDropdownsAsync(PayrollViewModel model)
    {
        model.PlantList = await _db.Plants.AsNoTracking()
            .OrderBy(p => p.PlantName)
            .Select(p => new SelectListItem
            {
                Value = p.PlantID.ToString(),
                Text = p.PlantName
            })
            .ToListAsync();
    }
}