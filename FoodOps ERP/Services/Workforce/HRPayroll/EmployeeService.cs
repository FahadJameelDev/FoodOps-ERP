using FoodOpsERP.Data;
using FoodOpsERP.Models.Workforce.HRPayroll.Entities;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _db;

    public EmployeeService(ApplicationDbContext db) => _db = db;

    public async Task<List<EmployeeViewModel>> GetAllAsync()
    {
        return await _db.Employees.AsNoTracking()
            .OrderBy(e => e.EmployeeCode)
            .Select(e => new EmployeeViewModel
            {
                EmployeeID = e.EmployeeID,
                PlantID = e.PlantID,
                PlantName = _db.Plants.Where(p => p.PlantID == e.PlantID).Select(p => p.PlantName).FirstOrDefault(),
                DepartmentID = e.DepartmentID,
                DepartmentName = _db.Departments.Where(d => d.DepartmentID == e.DepartmentID).Select(d => d.DepartmentName).FirstOrDefault(),
                ShiftID = e.ShiftID,
                ShiftName = _db.Shifts.Where(s => s.ShiftID == e.ShiftID).Select(s => s.ShiftName).FirstOrDefault(),
                EmployeeCode = e.EmployeeCode,
                FirstName = e.FirstName,
                LastName = e.LastName,
                CNIC = e.CNIC,
                Phone = e.Phone,
                Email = e.Email,
                Designation = e.Designation,
                JoiningDate = e.JoiningDate,
                // FIX: Property ka naam change kiya
                CurrentEmploymentStatus = e.EmploymentStatus,
                IsActive = e.IsActive
            }).ToListAsync();
    }

    public async Task<EmployeeViewModel?> GetByIdAsync(int id)
    {
        var e = await _db.Employees.AsNoTracking()
            .Include(x => x.Documents)
            .FirstOrDefaultAsync(x => x.EmployeeID == id);

        if (e == null) return null;

        return new EmployeeViewModel
        {
            EmployeeID = e.EmployeeID,
            PlantID = e.PlantID,
            DepartmentID = e.DepartmentID,
            ShiftID = e.ShiftID,
            UserID = e.UserID,
            EmployeeCode = e.EmployeeCode,
            FirstName = e.FirstName,
            LastName = e.LastName,
            CNIC = e.CNIC,
            Phone = e.Phone,
            Email = e.Email,
            Designation = e.Designation,
            JoiningDate = e.JoiningDate,
            LeavingDate = e.LeavingDate,
            // FIX: Property ka naam change kiya
            CurrentEmploymentStatus = e.EmploymentStatus,
            BankAccountNo = e.BankAccountNo,
            BankName = e.BankName,
            IsActive = e.IsActive,
            Documents = e.Documents.Select(d => new EmployeeDocumentViewModel
            {
                DocumentID = d.DocumentID,
                EmployeeID = d.EmployeeID,
                DocumentType = d.DocumentType,
                DocumentName = d.DocumentName,
                FilePath = d.FilePath,
                ExpiryDate = d.ExpiryDate,
                UploadedAt = d.UploadedAt
            }).ToList()
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(EmployeeViewModel model, int userId)
    {
        if (await _db.Employees.AnyAsync(e => e.EmployeeCode == model.EmployeeCode))
            return (false, "Employee Code already exists.", null);

        var entity = new Employee
        {
            PlantID = model.PlantID,
            DepartmentID = model.DepartmentID,
            ShiftID = model.ShiftID,
            UserID = model.UserID,
            EmployeeCode = model.EmployeeCode,
            FirstName = model.FirstName,
            LastName = model.LastName,
            CNIC = model.CNIC,
            Phone = model.Phone,
            Email = model.Email,
            Designation = model.Designation,
            JoiningDate = model.JoiningDate,
            LeavingDate = model.LeavingDate,
            // FIX: Property ka naam change kiya
            EmploymentStatus = model.CurrentEmploymentStatus,
            BankAccountNo = model.BankAccountNo,
            BankName = model.BankName,
            IsActive = model.IsActive,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Employees.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Employee created successfully.", entity.EmployeeID);
    }

    public async Task<(bool Success, string Message)> UpdateAsync(EmployeeViewModel model, int userId)
    {
        var entity = await _db.Employees.FindAsync(model.EmployeeID);
        if (entity == null) return (false, "Employee not found.");

        if (await _db.Employees.AnyAsync(e => e.EmployeeCode == model.EmployeeCode && e.EmployeeID != model.EmployeeID))
            return (false, "Employee Code already exists.");

        entity.PlantID = model.PlantID;
        entity.DepartmentID = model.DepartmentID;
        entity.ShiftID = model.ShiftID;
        entity.UserID = model.UserID;
        entity.EmployeeCode = model.EmployeeCode;
        entity.FirstName = model.FirstName;
        entity.LastName = model.LastName;
        entity.CNIC = model.CNIC;
        entity.Phone = model.Phone;
        entity.Email = model.Email;
        entity.Designation = model.Designation;
        entity.JoiningDate = model.JoiningDate;
        entity.LeavingDate = model.LeavingDate;
        // FIX: Property ka naam change kiya
        entity.EmploymentStatus = model.CurrentEmploymentStatus;
        entity.BankAccountNo = model.BankAccountNo;
        entity.BankName = model.BankName;
        entity.IsActive = model.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Employee updated successfully.");
    }

    public async Task<(bool Success, string Message)> ToggleActiveAsync(int id, int userId)
    {
        var entity = await _db.Employees.FindAsync(id);
        if (entity == null) return (false, "Employee not found.");

        entity.IsActive = !entity.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, entity.IsActive ? "Employee activated." : "Employee deactivated.");
    }
}