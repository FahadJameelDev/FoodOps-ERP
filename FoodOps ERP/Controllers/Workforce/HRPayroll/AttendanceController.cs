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

    // --- Attendance Logs ---
    public async Task<IActionResult> Index(DateTime? date = null)
    {
        var list = await _service.GetAllAttendanceAsync(date);
        ViewBag.Date = date?.ToString("yyyy-MM-dd") ?? DateTime.Today.ToString("yyyy-MM-dd");
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadLookupsAsync();
        return FormView("_AttendanceForm", "Create", new AttendanceViewModel { LogDate = DateTime.Today, Status = "Present" });
    }

    [HttpPost]
    public async Task<IActionResult> Create(AttendanceViewModel model)
    {
        if (!ModelState.IsValid) { await LoadLookupsAsync(); return Invalid("_AttendanceForm", "Create", model); }

        var result = await _service.CreateAttendanceAsync(model, GetCurrentUserId());
        if (!result.Success) { ModelState.AddModelError("", result.Message); await LoadLookupsAsync(); return Invalid("_AttendanceForm", "Create", model); }

        return Saved(result.Message, "Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var model = await _service.GetAttendanceByIdAsync(id);
        if (model == null) return NotFound();
        await LoadLookupsAsync();
        return FormView("_AttendanceForm", "Edit", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AttendanceViewModel model)
    {
        if (!ModelState.IsValid) { await LoadLookupsAsync(); return Invalid("_AttendanceForm", "Edit", model); }

        var result = await _service.UpdateAttendanceAsync(model, GetCurrentUserId());
        if (!result.Success) { ModelState.AddModelError("", result.Message); await LoadLookupsAsync(); return Invalid("_AttendanceForm", "Edit", model); }

        return Saved(result.Message, "Index");
    }

    // --- Attendance Corrections ---
    public async Task<IActionResult> Corrections(string? status = null)
    {
        var list = await _service.GetAllCorrectionsAsync(status);
        ViewBag.Status = status;
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> CreateCorrection(long? attendanceId = null)
    {
        await LoadCorrectionLookupsAsync();
        return FormView("_CorrectionForm", "CreateCorrection", new AttendanceCorrectionViewModel { AttendanceID = attendanceId ?? 0 });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCorrection(AttendanceCorrectionViewModel model)
    {
        if (!ModelState.IsValid) { await LoadCorrectionLookupsAsync(); return Invalid("_CorrectionForm", "CreateCorrection", model); }

        var result = await _service.CreateCorrectionAsync(model, GetCurrentUserId());
        if (!result.Success) { ModelState.AddModelError("", result.Message); await LoadCorrectionLookupsAsync(); return Invalid("_CorrectionForm", "CreateCorrection", model); }

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

    private async Task LoadLookupsAsync()
    {
        ViewBag.Employees = new SelectList(await _db.Employees.AsNoTracking().OrderBy(e => e.FirstName).ToListAsync(), "EmployeeID", "FirstName");
        ViewBag.Shifts = new SelectList(await _db.Shifts.AsNoTracking().OrderBy(s => s.ShiftName).ToListAsync(), "ShiftID", "ShiftName");
    }

    private async Task LoadCorrectionLookupsAsync()
    {
        ViewBag.AttendanceLogs = new SelectList(
            await _db.AttendanceLogs.AsNoTracking()
                .OrderByDescending(a => a.LogDate).Take(100)
                .Select(a => new { a.AttendanceID, Text = a.LogDate.ToString("dd MMM yyyy") + " - " + a.EmployeeID })
                .ToListAsync(),
            "AttendanceID", "Text");
    }
}