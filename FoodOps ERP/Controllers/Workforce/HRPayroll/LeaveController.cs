using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using FoodOpsERP.Services.Workforce.HRPayroll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Workforce.HRPayroll;

public class LeaveController : AppController
{
    private readonly ILeaveService _service;
    private readonly ApplicationDbContext _db;

    public LeaveController(ILeaveService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    // --- Leave Requests (Main Page) ---
    public async Task<IActionResult> Index(string? status = null)
    {
        var list = await _service.GetAllLeaveRequestsAsync(status);
        ViewBag.Status = status;
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new LeaveRequestViewModel { FromDate = DateTime.Today, ToDate = DateTime.Today, TotalDays = 1 };
        await PopulateDropdownsAsync(model); // ✅ Naya method call kiya
        return FormView("_LeaveRequestForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LeaveRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync(model);
            return Invalid("_LeaveRequestForm", "Create", model);
        }

        var result = await _service.CreateLeaveRequestAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_LeaveRequestForm", "Create", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _service.ApproveLeaveRequestAsync(id, GetCurrentUserId(), true, null);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id, string reason)
    {
        var result = await _service.ApproveLeaveRequestAsync(id, GetCurrentUserId(), false, reason);
        return result.Success ? Saved(result.Message, "Index") : Failed(result.Message);
    }

    // --- Leave Types ---
    public async Task<IActionResult> LeaveTypes()
    {
        var list = await _service.GetAllLeaveTypesAsync();
        return View(list);
    }

    [HttpGet]
    public IActionResult CreateType()
    {
        return FormView("_LeaveTypeForm", "CreateType", new LeaveTypeViewModel { IsPaid = true, IsActive = true });
    }

    [HttpPost]
    public async Task<IActionResult> CreateType(LeaveTypeViewModel model)
    {
        if (!ModelState.IsValid) return Invalid("_LeaveTypeForm", "CreateType", model);

        var result = await _service.CreateLeaveTypeAsync(model, GetCurrentUserId());
        if (!result.Success) { ModelState.AddModelError("", result.Message); return Invalid("_LeaveTypeForm", "CreateType", model); }
        return Saved(result.Message, "LeaveTypes");
    }

    [HttpGet]
    public async Task<IActionResult> EditType(int id)
    {
        var model = await _service.GetLeaveTypeByIdAsync(id);
        if (model == null) return NotFound();
        return FormView("_LeaveTypeForm", "EditType", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditType(LeaveTypeViewModel model)
    {
        if (!ModelState.IsValid) return Invalid("_LeaveTypeForm", "EditType", model);

        var result = await _service.UpdateLeaveTypeAsync(model, GetCurrentUserId());
        if (!result.Success) { ModelState.AddModelError("", result.Message); return Invalid("_LeaveTypeForm", "EditType", model); }
        return Saved(result.Message, "LeaveTypes");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleType(int id)
    {
        var result = await _service.ToggleLeaveTypeActiveAsync(id, GetCurrentUserId());
        return result.Success ? Saved(result.Message, "LeaveTypes") : Failed(result.Message);
    }

    // ✅ Naya Method (Purana LoadLookupsAsync hata dein)
    private async Task PopulateDropdownsAsync(LeaveRequestViewModel model)
    {
        model.EmployeeList = await _db.Employees.AsNoTracking()
            .OrderBy(e => e.FirstName)
            .Select(e => new SelectListItem
            {
                Value = e.EmployeeID.ToString(),
                Text = $"{e.EmployeeCode} - {e.FirstName} {e.LastName}"
            })
            .ToListAsync();

        model.LeaveTypeList = await _db.LeaveTypes.AsNoTracking()
            .OrderBy(t => t.LeaveTypeName)
            .Select(t => new SelectListItem
            {
                Value = t.LeaveTypeID.ToString(),
                Text = t.LeaveTypeName
            })
            .ToListAsync();
    }
}