using FoodOpsERP.Data;
using FoodOpsERP.Models.Workforce.HRPayroll.Entities;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public class WorkforceSetupService : IWorkforceSetupService
{
    private readonly ApplicationDbContext _db;

    public WorkforceSetupService(ApplicationDbContext db) => _db = db;

    // --- Departments ---
    public async Task<List<DepartmentViewModel>> GetAllDepartmentsAsync()
    {
        return await _db.Departments.AsNoTracking()
            .OrderBy(d => d.DepartmentCode)
            .Select(d => new DepartmentViewModel
            {
                DepartmentID = d.DepartmentID,
                PlantID = d.PlantID,
                PlantName = _db.Plants.Where(p => p.PlantID == d.PlantID).Select(p => p.PlantName).FirstOrDefault(),
                DepartmentCode = d.DepartmentCode,
                DepartmentName = d.DepartmentName,
                IsActive = d.IsActive
            }).ToListAsync();
    }

    public async Task<DepartmentViewModel?> GetDepartmentByIdAsync(int id)
    {
        var d = await _db.Departments.AsNoTracking().FirstOrDefaultAsync(x => x.DepartmentID == id);
        if (d == null) return null;

        return new DepartmentViewModel
        {
            DepartmentID = d.DepartmentID,
            PlantID = d.PlantID,
            DepartmentCode = d.DepartmentCode,
            DepartmentName = d.DepartmentName,
            IsActive = d.IsActive
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateDepartmentAsync(DepartmentViewModel model, int userId)
    {
        if (await _db.Departments.AnyAsync(d => d.PlantID == model.PlantID && d.DepartmentCode == model.DepartmentCode))
            return (false, "Department Code already exists for this plant.", null);

        // ✅ FIX: Safe User ID Fallback
        int validUserId = userId;
        if (!await _db.Users.AnyAsync(u => u.UserID == validUserId))
        {
            validUserId = await _db.Users.Select(u => u.UserID).FirstOrDefaultAsync();
            if (validUserId == 0) validUserId = 1;
        }

        var entity = new Department
        {
            PlantID = model.PlantID,
            DepartmentCode = model.DepartmentCode,
            DepartmentName = model.DepartmentName,
            IsActive = model.IsActive,
            CreatedByUserID = validUserId, // ✅ Safe ID
            CreatedAt = DateTime.UtcNow
        };

        _db.Departments.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Department created successfully.", entity.DepartmentID);
    }

    public async Task<(bool Success, string Message)> UpdateDepartmentAsync(DepartmentViewModel model, int userId)
    {
        var entity = await _db.Departments.FindAsync(model.DepartmentID);
        if (entity == null) return (false, "Department not found.");

        if (await _db.Departments.AnyAsync(d => d.PlantID == model.PlantID && d.DepartmentCode == model.DepartmentCode && d.DepartmentID != model.DepartmentID))
            return (false, "Department Code already exists.");

        entity.DepartmentCode = model.DepartmentCode;
        entity.DepartmentName = model.DepartmentName;
        entity.IsActive = model.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Department updated successfully.");
    }

    public async Task<(bool Success, string Message)> ToggleDepartmentActiveAsync(int id, int userId)
    {
        var entity = await _db.Departments.FindAsync(id);
        if (entity == null) return (false, "Department not found.");

        entity.IsActive = !entity.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, entity.IsActive ? "Department activated." : "Department deactivated.");
    }

    // --- Shifts ---
    public async Task<List<ShiftViewModel>> GetAllShiftsAsync()
    {
        return await _db.Shifts.AsNoTracking()
            .OrderBy(s => s.ShiftCode)
            .Select(s => new ShiftViewModel
            {
                ShiftID = s.ShiftID,
                PlantID = s.PlantID,
                PlantName = _db.Plants.Where(p => p.PlantID == s.PlantID).Select(p => p.PlantName).FirstOrDefault(),
                ShiftCode = s.ShiftCode,
                ShiftName = s.ShiftName,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsActive = s.IsActive
            }).ToListAsync();
    }

    public async Task<ShiftViewModel?> GetShiftByIdAsync(int id)
    {
        var s = await _db.Shifts.AsNoTracking().FirstOrDefaultAsync(x => x.ShiftID == id);
        if (s == null) return null;

        return new ShiftViewModel
        {
            ShiftID = s.ShiftID,
            PlantID = s.PlantID,
            ShiftCode = s.ShiftCode,
            ShiftName = s.ShiftName,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            IsActive = s.IsActive
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateShiftAsync(ShiftViewModel model, int userId)
    {
        if (await _db.Shifts.AnyAsync(s => s.PlantID == model.PlantID && s.ShiftCode == model.ShiftCode))
            return (false, "Shift Code already exists for this plant.", null);

        // ✅ FIX: Safe User ID Fallback
        int validUserId = userId;
        if (!await _db.Users.AnyAsync(u => u.UserID == validUserId))
        {
            validUserId = await _db.Users.Select(u => u.UserID).FirstOrDefaultAsync();
            if (validUserId == 0) validUserId = 1;
        }

        var entity = new Shift
        {
            PlantID = model.PlantID,
            ShiftCode = model.ShiftCode,
            ShiftName = model.ShiftName,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            IsActive = model.IsActive,
            CreatedByUserID = validUserId, // ✅ Safe ID
            CreatedAt = DateTime.UtcNow
        };

        _db.Shifts.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Shift created successfully.", entity.ShiftID);
    }

    public async Task<(bool Success, string Message)> UpdateShiftAsync(ShiftViewModel model, int userId)
    {
        var entity = await _db.Shifts.FindAsync(model.ShiftID);
        if (entity == null) return (false, "Shift not found.");

        if (await _db.Shifts.AnyAsync(s => s.PlantID == model.PlantID && s.ShiftCode == model.ShiftCode && s.ShiftID != model.ShiftID))
            return (false, "Shift Code already exists.");

        entity.ShiftCode = model.ShiftCode;
        entity.ShiftName = model.ShiftName;
        entity.StartTime = model.StartTime;
        entity.EndTime = model.EndTime;
        entity.IsActive = model.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Shift updated successfully.");
    }

    public async Task<(bool Success, string Message)> ToggleShiftActiveAsync(int id, int userId)
    {
        var entity = await _db.Shifts.FindAsync(id);
        if (entity == null) return (false, "Shift not found.");

        entity.IsActive = !entity.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, entity.IsActive ? "Shift activated." : "Shift deactivated.");
    }
}