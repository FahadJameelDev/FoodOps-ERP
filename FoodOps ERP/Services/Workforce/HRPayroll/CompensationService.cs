using FoodOpsERP.Data;
using FoodOpsERP.Models.Workforce.HRPayroll.Entities;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public class CompensationService : ICompensationService
{
    private readonly ApplicationDbContext _db;

    public CompensationService(ApplicationDbContext db) => _db = db;

    // --- Salary Structures ---
    public async Task<List<SalaryStructureViewModel>> GetAllStructuresAsync()
    {
        return await _db.SalaryStructures.AsNoTracking()
            .OrderBy(s => s.StructureCode)
            .Select(s => new SalaryStructureViewModel
            {
                StructureID = s.StructureID,
                StructureCode = s.StructureCode,
                StructureName = s.StructureName,
                BasicSalary = s.BasicSalary,
                HousingAllowance = s.HousingAllowance,
                TransportAllowance = s.TransportAllowance,
                OtherAllowance = s.OtherAllowance,
                IsActive = s.IsActive
            }).ToListAsync();
    }

    public async Task<SalaryStructureViewModel?> GetStructureByIdAsync(int id)
    {
        var s = await _db.SalaryStructures.AsNoTracking().FirstOrDefaultAsync(x => x.StructureID == id);
        if (s == null) return null;

        return new SalaryStructureViewModel
        {
            StructureID = s.StructureID,
            StructureCode = s.StructureCode,
            StructureName = s.StructureName,
            BasicSalary = s.BasicSalary,
            HousingAllowance = s.HousingAllowance,
            TransportAllowance = s.TransportAllowance,
            OtherAllowance = s.OtherAllowance,
            IsActive = s.IsActive
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateStructureAsync(SalaryStructureViewModel model, int userId)
    {
        if (await _db.SalaryStructures.AnyAsync(s => s.StructureCode == model.StructureCode))
            return (false, "Structure Code already exists.", null);

        // ✅ FIX: Safe User ID Fallback
        int validUserId = userId;
        if (!await _db.Users.AnyAsync(u => u.UserID == validUserId))
        {
            validUserId = await _db.Users.Select(u => u.UserID).FirstOrDefaultAsync();
            if (validUserId == 0) validUserId = 1;
        }

        var entity = new SalaryStructure
        {
            StructureCode = model.StructureCode,
            StructureName = model.StructureName,
            BasicSalary = model.BasicSalary,
            HousingAllowance = model.HousingAllowance,
            TransportAllowance = model.TransportAllowance,
            OtherAllowance = model.OtherAllowance,
            IsActive = model.IsActive,
            CreatedByUserID = validUserId, // ✅ Safe ID
            CreatedAt = DateTime.UtcNow
        };

        _db.SalaryStructures.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Salary Structure created successfully.", entity.StructureID);
    }

    public async Task<(bool Success, string Message)> UpdateStructureAsync(SalaryStructureViewModel model, int userId)
    {
        var entity = await _db.SalaryStructures.FindAsync(model.StructureID);
        if (entity == null) return (false, "Structure not found.");

        if (await _db.SalaryStructures.AnyAsync(s => s.StructureCode == model.StructureCode && s.StructureID != model.StructureID))
            return (false, "Structure Code already exists.");

        entity.StructureCode = model.StructureCode;
        entity.StructureName = model.StructureName;
        entity.BasicSalary = model.BasicSalary;
        entity.HousingAllowance = model.HousingAllowance;
        entity.TransportAllowance = model.TransportAllowance;
        entity.OtherAllowance = model.OtherAllowance;
        entity.IsActive = model.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Salary Structure updated successfully.");
    }

    public async Task<(bool Success, string Message)> ToggleStructureActiveAsync(int id, int userId)
    {
        var entity = await _db.SalaryStructures.FindAsync(id);
        if (entity == null) return (false, "Structure not found.");

        entity.IsActive = !entity.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, entity.IsActive ? "Structure activated." : "Structure deactivated.");
    }

    // --- Employee Salaries ---
    public async Task<List<EmployeeSalaryViewModel>> GetAllEmployeeSalariesAsync(int? employeeId = null)
    {
        var query = _db.EmployeeSalaries.AsNoTracking().AsQueryable();

        if (employeeId.HasValue)
            query = query.Where(s => s.EmployeeID == employeeId.Value);

        return await query
            .OrderByDescending(s => s.EffectiveFrom)
            .Select(s => new EmployeeSalaryViewModel
            {
                EmployeeSalaryID = s.EmployeeSalaryID,
                EmployeeID = s.EmployeeID,
                EmployeeName = _db.Employees.Where(e => e.EmployeeID == s.EmployeeID).Select(e => e.FirstName + " " + e.LastName).FirstOrDefault(),
                StructureID = s.StructureID,
                StructureName = _db.SalaryStructures.Where(st => st.StructureID == s.StructureID).Select(st => st.StructureName).FirstOrDefault(),
                EffectiveFrom = s.EffectiveFrom,
                EffectiveTo = s.EffectiveTo,
                BasicSalary = s.BasicSalary,
                HousingAllowance = s.HousingAllowance,
                TransportAllowance = s.TransportAllowance,
                OtherAllowance = s.OtherAllowance,
                GrossSalary = s.GrossSalary,
                Status = s.Status
            }).ToListAsync();
    }

    public async Task<EmployeeSalaryViewModel?> GetEmployeeSalaryByIdAsync(int id)
    {
        var s = await _db.EmployeeSalaries.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeSalaryID == id);
        if (s == null) return null;

        return new EmployeeSalaryViewModel
        {
            EmployeeSalaryID = s.EmployeeSalaryID,
            EmployeeID = s.EmployeeID,
            StructureID = s.StructureID,
            EffectiveFrom = s.EffectiveFrom,
            EffectiveTo = s.EffectiveTo,
            BasicSalary = s.BasicSalary,
            HousingAllowance = s.HousingAllowance,
            TransportAllowance = s.TransportAllowance,
            OtherAllowance = s.OtherAllowance,
            GrossSalary = s.GrossSalary,
            Status = s.Status
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateEmployeeSalaryAsync(EmployeeSalaryViewModel model, int userId)
    {
        // Mark old salary as superseded
        var oldSalaries = await _db.EmployeeSalaries
            .Where(s => s.EmployeeID == model.EmployeeID && s.Status == "Active")
            .ToListAsync();
        foreach (var old in oldSalaries)
        {
            old.Status = "Superseded";
            old.EffectiveTo = model.EffectiveFrom.AddDays(-1);
        }

        // ✅ FIX: Safe User ID Fallback
        int validUserId = userId;
        if (!await _db.Users.AnyAsync(u => u.UserID == validUserId))
        {
            validUserId = await _db.Users.Select(u => u.UserID).FirstOrDefaultAsync();
            if (validUserId == 0) validUserId = 1;
        }

        var gross = model.BasicSalary + model.HousingAllowance + model.TransportAllowance + model.OtherAllowance;

        var entity = new EmployeeSalary
        {
            EmployeeID = model.EmployeeID,
            StructureID = model.StructureID,
            EffectiveFrom = model.EffectiveFrom,
            EffectiveTo = model.EffectiveTo,
            BasicSalary = model.BasicSalary,
            HousingAllowance = model.HousingAllowance,
            TransportAllowance = model.TransportAllowance,
            OtherAllowance = model.OtherAllowance,
            GrossSalary = gross,
            Status = "Active",
            CreatedByUserID = validUserId, // ✅ Safe ID
            CreatedAt = DateTime.UtcNow
        };

        _db.EmployeeSalaries.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Employee Salary set successfully.", entity.EmployeeSalaryID);
    }

    public async Task<(bool Success, string Message)> UpdateEmployeeSalaryAsync(EmployeeSalaryViewModel model, int userId)
    {
        var entity = await _db.EmployeeSalaries.FindAsync(model.EmployeeSalaryID);
        if (entity == null) return (false, "Salary record not found.");
        if (entity.Status == "Superseded") return (false, "Cannot edit superseded salary.");

        entity.EffectiveFrom = model.EffectiveFrom;
        entity.EffectiveTo = model.EffectiveTo;
        entity.BasicSalary = model.BasicSalary;
        entity.HousingAllowance = model.HousingAllowance;
        entity.TransportAllowance = model.TransportAllowance;
        entity.OtherAllowance = model.OtherAllowance;
        entity.GrossSalary = model.BasicSalary + model.HousingAllowance + model.TransportAllowance + model.OtherAllowance;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Employee Salary updated.");
    }
}