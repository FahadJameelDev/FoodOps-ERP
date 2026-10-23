using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using FoodOpsERP.Services.Workforce.HRPayroll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Workforce.HRPayroll;

public class WorkforceSetupController : AppController
{
    private readonly IWorkforceSetupService _service;
    private readonly ApplicationDbContext _db;

    public WorkforceSetupController(IWorkforceSetupService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    // --- Departments ---
    public async Task<IActionResult> Index()
    {
        var list = await _service.GetAllDepartmentsAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> CreateDepartment()
    {
        var model = new DepartmentViewModel { IsActive = true };
        await PopulateDropdownsAsync(model); // ✅ Naya method call kiya
        return FormView("_DepartmentForm", "CreateDepartment", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment(DepartmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_DepartmentForm", "CreateDepartment", model);
        }

        var result = await _service.CreateDepartmentAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_DepartmentForm", "CreateDepartment", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditDepartment(int id)
    {
        var model = await _service.GetDepartmentByIdAsync(id);
        if (model == null) return NotFound();
        await PopulateDropdownsAsync(model);
        return FormView("_DepartmentForm", "EditDepartment", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditDepartment(DepartmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_DepartmentForm", "EditDepartment", model);
        }

        var result = await _service.UpdateDepartmentAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_DepartmentForm", "EditDepartment", model);
        }

        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleDepartment(int id)
    {
        var result = await _service.ToggleDepartmentActiveAsync(id, GetCurrentUserId());
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    // --- Shifts ---
    public async Task<IActionResult> Shifts()
    {
        var list = await _service.GetAllShiftsAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> CreateShift()
    {
        var model = new ShiftViewModel { IsActive = true };
        await PopulateDropdownsAsync(model);
        return FormView("_ShiftForm", "CreateShift", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateShift(ShiftViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_ShiftForm", "CreateShift", model);
        }

        var result = await _service.CreateShiftAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_ShiftForm", "CreateShift", model);
        }

        return Saved(result.Message, "Shifts");
    }

    [HttpGet]
    public async Task<IActionResult> EditShift(int id)
    {
        var model = await _service.GetShiftByIdAsync(id);
        if (model == null) return NotFound();
        await PopulateDropdownsAsync(model);
        return FormView("_ShiftForm", "EditShift", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditShift(ShiftViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_ShiftForm", "EditShift", model);
        }

        var result = await _service.UpdateShiftAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_ShiftForm", "EditShift", model);
        }

        return Saved(result.Message, "Shifts");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleShift(int id)
    {
        var result = await _service.ToggleShiftActiveAsync(id, GetCurrentUserId());
        return result.Success ? Saved(result.Message, "Shifts") : Failed(result.Message);
    }

    // ✅ Naya Method (Purana LoadPlantsAsync hata dein)
    private async Task PopulateDropdownsAsync(object model)
    {
        var plants = await _db.Plants.AsNoTracking()
            .OrderBy(p => p.PlantName)
            .Select(p => new SelectListItem { Value = p.PlantID.ToString(), Text = p.PlantName })
            .ToListAsync();

        // Hum dono ViewModels (Department aur Shift) ke liye ek hi method use kar rahe hain
        if (model is DepartmentViewModel deptModel)
        {
            deptModel.PlantList = plants;
        }
        else if (model is ShiftViewModel shiftModel)
        {
            shiftModel.PlantList = plants;
        }
    }
}