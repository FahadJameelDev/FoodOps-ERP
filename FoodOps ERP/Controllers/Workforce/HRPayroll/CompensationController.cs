using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using FoodOpsERP.Services.Workforce.HRPayroll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Workforce.HRPayroll;

public class CompensationController : AppController
{
    private readonly ICompensationService _service;
    private readonly ApplicationDbContext _db;

    public CompensationController(ICompensationService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    // --- Salary Structures ---
    public async Task<IActionResult> Index()
    {
        var list = await _service.GetAllStructuresAsync();
        return View(list);
    }

    [HttpGet]
    public IActionResult CreateStructure()
    {
        return FormView("_StructureForm", "CreateStructure", new SalaryStructureViewModel { IsActive = true });
    }

    [HttpPost]
    public async Task<IActionResult> CreateStructure(SalaryStructureViewModel model)
    {
        if (!ModelState.IsValid) return Invalid("_StructureForm", "CreateStructure", model);

        var result = await _service.CreateStructureAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return Invalid("_StructureForm", "CreateStructure", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditStructure(int id)
    {
        var model = await _service.GetStructureByIdAsync(id);
        if (model == null) return NotFound();
        return FormView("_StructureForm", "EditStructure", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditStructure(SalaryStructureViewModel model)
    {
        if (!ModelState.IsValid) return Invalid("_StructureForm", "EditStructure", model);

        var result = await _service.UpdateStructureAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return Invalid("_StructureForm", "EditStructure", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStructure(int id)
    {
        var result = await _service.ToggleStructureActiveAsync(id, GetCurrentUserId());
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    // --- Employee Salaries ---
    public async Task<IActionResult> EmployeeSalaries()
    {
        var list = await _service.GetAllEmployeeSalariesAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> CreateEmployeeSalary()
    {
        var model = new EmployeeSalaryViewModel { EffectiveFrom = DateTime.Today };
        await PopulateDropdownsAsync(model); // ✅ Naya method call kiya
        return FormView("_EmployeeSalaryForm", "CreateEmployeeSalary", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployeeSalary(EmployeeSalaryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_EmployeeSalaryForm", "CreateEmployeeSalary", model);
        }

        var result = await _service.CreateEmployeeSalaryAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_EmployeeSalaryForm", "CreateEmployeeSalary", model);
        }
        return Saved(result.Message, "EmployeeSalaries");
    }

    // ✅ Naya Method (Purana LoadLookupsAsync hata dein)
    private async Task PopulateDropdownsAsync(EmployeeSalaryViewModel model)
    {
        model.EmployeeList = await _db.Employees.AsNoTracking()
            .OrderBy(e => e.FirstName)
            .Select(e => new SelectListItem
            {
                Value = e.EmployeeID.ToString(),
                Text = $"{e.EmployeeCode} - {e.FirstName} {e.LastName}"
            })
            .ToListAsync();

        model.StructureList = await _db.SalaryStructures.AsNoTracking()
            .OrderBy(s => s.StructureName)
            .Select(s => new SelectListItem
            {
                Value = s.StructureID.ToString(),
                Text = s.StructureName
            })
            .ToListAsync();
    }
}