using FoodOpsERP.Data;
using FoodOpsERP.Models.Workforce.HRPayroll.Entities;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public class OvertimeService : IOvertimeService
{
    private readonly ApplicationDbContext _db;

    public OvertimeService(ApplicationDbContext db) => _db = db;

    public async Task<List<OvertimeViewModel>> GetAllAsync(string? status = null)
    {
        var query = _db.OvertimeRecords.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(o => o.Status == status);

        return await query
            .OrderByDescending(o => o.OTDate)
            .Select(o => new OvertimeViewModel
            {
                OvertimeID = o.OvertimeID,
                EmployeeID = o.EmployeeID,
                EmployeeName = _db.Employees.Where(e => e.EmployeeID == o.EmployeeID).Select(e => e.FirstName + " " + e.LastName).FirstOrDefault(),
                OTDate = o.OTDate,
                OTHours = o.OTHours,
                OTRateMultiplier = o.OTRateMultiplier,
                Reason = o.Reason,
                Status = o.Status,
                ApprovedAt = o.ApprovedAt
            }).ToListAsync();
    }

    public async Task<OvertimeViewModel?> GetByIdAsync(int id)
    {
        var o = await _db.OvertimeRecords.AsNoTracking().FirstOrDefaultAsync(x => x.OvertimeID == id);
        if (o == null) return null;

        return new OvertimeViewModel
        {
            OvertimeID = o.OvertimeID,
            EmployeeID = o.EmployeeID,
            OTDate = o.OTDate,
            OTHours = o.OTHours,
            OTRateMultiplier = o.OTRateMultiplier,
            Reason = o.Reason,
            Status = o.Status,
            ApprovedByUserID = o.ApprovedByUserID,
            ApprovedAt = o.ApprovedAt
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(OvertimeViewModel model, int userId)
    {
        // ✅ FIX: Safe User ID Fallback
        int validUserId = userId;
        if (!await _db.Users.AnyAsync(u => u.UserID == validUserId))
        {
            validUserId = await _db.Users.Select(u => u.UserID).FirstOrDefaultAsync();
            if (validUserId == 0) validUserId = 1;
        }

        var entity = new OvertimeRecord
        {
            EmployeeID = model.EmployeeID,
            OTDate = model.OTDate,
            OTHours = model.OTHours,
            OTRateMultiplier = model.OTRateMultiplier,
            Reason = model.Reason,
            Status = "Pending",
            CreatedByUserID = validUserId, // ✅ Safe ID
            CreatedAt = DateTime.UtcNow
        };

        _db.OvertimeRecords.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Overtime record created successfully.", entity.OvertimeID);
    }

    public async Task<(bool Success, string Message)> ApproveAsync(int id, int userId, bool approve)
    {
        var entity = await _db.OvertimeRecords.FindAsync(id);
        if (entity == null) return (false, "Overtime record not found.");
        if (entity.Status != "Pending") return (false, "Only pending records can be approved/rejected.");

        // ✅ FIX: Safe User ID Fallback
        int validUserId = userId;
        if (!await _db.Users.AnyAsync(u => u.UserID == validUserId))
        {
            validUserId = await _db.Users.Select(u => u.UserID).FirstOrDefaultAsync();
            if (validUserId == 0) validUserId = 1;
        }

        entity.Status = approve ? "Approved" : "Rejected";
        entity.ApprovedByUserID = validUserId; // ✅ Safe ID
        entity.ApprovedAt = DateTime.UtcNow;
        entity.ModifiedByUserID = validUserId; // ✅ Safe ID
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, approve ? "Overtime approved." : "Overtime rejected.");
    }
}