using FoodOpsERP.Data;
using FoodOpsERP.Infrastructure;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using FoodOpsERP.Services.Workforce.HRPayroll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Controllers.Workforce.HRPayroll;

public class AttendanceController : AppController
{
    private readonly IAttendanceService _service;
    private readonly ApplicationDbContext _db;

    public AttendanceController(IAttendanceService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    public async Task<IActionResult> Index(DateTime? date = null)
    {
        var list = await _service.GetAllAttendanceAsync(date);
        ViewBag.Date = date?.ToString("yyyy-MM-dd") ?? DateTime.Today.ToString("yyyy-MM-dd");
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new AttendanceViewModel { LogDate = DateTime.Today, Status = "Present" };
        await PopulateDropdownsAsync(model); // ✅ Naya method call kiya
        return FormView("_AttendanceForm", "Create", model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AttendanceViewModel model)
    {
        if (!ModelState.IsValid) { await PopulateDropdownsAsync(model); return Invalid("_AttendanceForm", "Create", model); }

        var result = await _service.CreateAttendanceAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_AttendanceForm", "Create", model);
        }
        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var model = await _service.GetAttendanceByIdAsync(id);
        if (model == null) return NotFound();
        await PopulateDropdownsAsync(model);
        return FormView("_AttendanceForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AttendanceViewModel model)
    {
        if (!ModelState.IsValid) { await PopulateDropdownsAsync(model); return Invalid("_AttendanceForm", "Edit", model); }

        var result = await _service.UpdateAttendanceAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateDropdownsAsync(model);
            return Invalid("_AttendanceForm", "Edit", model);
        }
        return Saved(result.Message, "Index");
    }

    // --- Corrections ---
    public async Task<IActionResult> Corrections(string? status = null)
    {
        var list = await _service.GetAllCorrectionsAsync(status);
        ViewBag.Status = status;
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> CreateCorrection(long? attendanceId = null)
    {
        var model = new AttendanceCorrectionViewModel { AttendanceID = attendanceId ?? 0 };
        await PopulateCorrectionDropdownsAsync(model); // ✅ Naya method
        return FormView("_CorrectionForm", "CreateCorrection", model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCorrection(AttendanceCorrectionViewModel model)
    {
        if (!ModelState.IsValid) { await PopulateCorrectionDropdownsAsync(model); return Invalid("_CorrectionForm", "CreateCorrection", model); }

        var result = await _service.CreateCorrectionAsync(model, GetCurrentUserId());
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            await PopulateCorrectionDropdownsAsync(model);
            return Invalid("_CorrectionForm", "CreateCorrection", model);
        }
        return Saved(result.Message, "Corrections");
    }

    [HttpPost]
    public async Task<IActionResult> ApproveCorrection(int id)
    {
        var result = await _service.ApproveCorrectionAsync(id, GetCurrentUserId(), true);
        return result.Success ? Saved(result.Message, "Corrections") : Failed(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> RejectCorrection(int id)
    {
        var result = await _service.ApproveCorrectionAsync(id, GetCurrentUserId(), false);
        return result.Success ? Saved(result.Message, "Corrections") : Failed(result.Message);
    }

    // ✅ Naya Methods (Purana LoadLookupsAsync hata dein)
    private async Task PopulateDropdownsAsync(AttendanceViewModel model)
    {
        model.EmployeeList = await _db.Employees.AsNoTracking()
            .OrderBy(e => e.FirstName)
            .Select(e => new SelectListItem
            {
                Value = e.EmployeeID.ToString(),
                Text = $"{e.EmployeeCode} - {e.FirstName} {e.LastName}"
            })
            .ToListAsync();

        model.ShiftList = await _db.Shifts.AsNoTracking()
            .OrderBy(s => s.ShiftName)
            .Select(s => new SelectListItem { Value = s.ShiftID.ToString(), Text = s.ShiftName })
            .ToListAsync();
    }

    private async Task PopulateCorrectionDropdownsAsync(AttendanceCorrectionViewModel model)
    {
        model.AttendanceLogList = await _db.AttendanceLogs.AsNoTracking()
            .OrderByDescending(a => a.LogDate)
            .Take(100)
            .Select(a => new SelectListItem
            {
                Value = a.AttendanceID.ToString(),
                Text = $"{a.LogDate:dd MMM yyyy} - {_db.Employees.Where(e => e.EmployeeID == a.EmployeeID).Select(e => e.FirstName + " " + e.LastName).FirstOrDefault()}"
            })
            .ToListAsync();
    }
}