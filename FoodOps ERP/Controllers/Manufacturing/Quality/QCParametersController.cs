using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Manufacturing.Quality.ViewModels;
using FoodOpsERP.Services.Manufacturing.Quality;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Manufacturing.Quality;

public class QCParametersController : AppController
{
    private readonly IQCParameterService _service;
    private readonly ApplicationDbContext _db;

    public QCParametersController(IQCParameterService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _service.GetAllAsync();
        return View(list);
    }

    public async Task<IActionResult> Records()
    {
        var list = await _service.GetAllAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new QCParameterViewModel { IsActive = true, ParameterType = "Numeric" };
        await PopulateDropdownsAsync(model); // ✅ Naya method call kiya
        return FormView("_ParameterForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(QCParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_ParameterForm", "Create", model);
        }

        var result = await _service.CreateAsync(model, 1);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_ParameterForm", "Create", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _service.GetByIdAsync(id);
        if (model == null) return NotFound();

        await PopulateDropdownsAsync(model);
        return FormView("_ParameterForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(QCParameterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_ParameterForm", "Edit", model);
        }

        var result = await _service.UpdateAsync(model, 1);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_ParameterForm", "Edit", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var result = await _service.ToggleActiveAsync(id, 1);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    // ✅ Naya Method (Purana LoadLookupsAsync hata dein)
    private async Task PopulateDropdownsAsync(QCParameterViewModel model)
    {
        model.UOMList = await _db.UnitsOfMeasure.AsNoTracking()
            .OrderBy(u => u.UOMName)
            .Select(u => new SelectListItem { Value = u.UOMID.ToString(), Text = u.UOMName })
            .ToListAsync();
    }
}