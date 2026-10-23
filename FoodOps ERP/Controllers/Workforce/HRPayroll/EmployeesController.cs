using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using FoodOpsERP.Services.Workforce.HRPayroll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Workforce.HRPayroll;

public class EmployeesController : AppController
{
    private readonly IEmployeeService _service;
    private readonly ApplicationDbContext _db;

    public EmployeesController(IEmployeeService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _service.GetAllAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new EmployeeViewModel
        {
            JoiningDate = DateTime.Today,
            CurrentEmploymentStatus = "Active",
            IsActive = true
        };
        await PopulateDropdownsAsync(model); // ✅ Naya method call kiya
        return FormView("_EmployeeForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_EmployeeForm", "Create", model);
        }

        var result = await _service.CreateAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_EmployeeForm", "Create", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();

        await PopulateDropdownsAsync(model);
        return FormView("_EmployeeForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_EmployeeForm", "Edit", model);
        }

        var result = await _service.UpdateAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_EmployeeForm", "Edit", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var result = await _service.ToggleActiveAsync(id, GetCurrentUserId());
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    // ✅ Naya Method (Purana LoadLookupsAsync hata dein)
    private async Task PopulateDropdownsAsync(EmployeeViewModel model)
    {
        model.PlantList = await _db.Plants.AsNoTracking()
            .OrderBy(p => p.PlantName)
            .Select(p => new SelectListItem { Value = p.PlantID.ToString(), Text = p.PlantName })
            .ToListAsync();

        model.DepartmentList = await _db.Departments.AsNoTracking()
            .OrderBy(d => d.DepartmentName)
            .Select(d => new SelectListItem { Value = d.DepartmentID.ToString(), Text = d.DepartmentName })
            .ToListAsync();

        model.ShiftList = await _db.Shifts.AsNoTracking()
            .OrderBy(s => s.ShiftName)
            .Select(s => new SelectListItem { Value = s.ShiftID.ToString(), Text = s.ShiftName })
            .ToListAsync();
    }
}