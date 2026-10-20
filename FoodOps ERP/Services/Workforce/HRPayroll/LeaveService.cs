using FoodOpsERP.Data;
using FoodOpsERP.Models.Workforce.HRPayroll.Entities;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public class LeaveService : ILeaveService
{
    private readonly ApplicationDbContext _db;

    public LeaveService(ApplicationDbContext db) => _db = db;

    // --- Leave Types ---
    public async Task<List<LeaveTypeViewModel>> GetAllLeaveTypesAsync()
    {
        return await _db.LeaveTypes.AsNoTracking()
            .OrderBy(t => t.LeaveTypeCode)
            .Select(t => new LeaveTypeViewModel
            {
                LeaveTypeID = t.LeaveTypeID,
                LeaveTypeCode = t.LeaveTypeCode,
                LeaveTypeName = t.LeaveTypeName,
                IsPaid = t.IsPaid,
                MaxDaysPerYear = t.MaxDaysPerYear,
                CarryForward = t.CarryForward,
                IsActive = t.IsActive
            }).ToListAsync();
    }

    public async Task<LeaveTypeViewModel?> GetLeaveTypeByIdAsync(int id)
    {
        var t = await _db.LeaveTypes.AsNoTracking().FirstOrDefaultAsync(x => x.LeaveTypeID == id);
        if (t == null) return null;

        return new LeaveTypeViewModel
        {
            LeaveTypeID = t.LeaveTypeID,
            LeaveTypeCode = t.LeaveTypeCode,
            LeaveTypeName = t.LeaveTypeName,
            IsPaid = t.IsPaid,
            MaxDaysPerYear = t.MaxDaysPerYear,
            CarryForward = t.CarryForward,
            IsActive = t.IsActive
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateLeaveTypeAsync(LeaveTypeViewModel model, int userId)
    {
        if (await _db.LeaveTypes.AnyAsync(t => t.LeaveTypeCode == model.LeaveTypeCode))
            return (false, "Leave Type Code already exists.", null);

        var entity = new LeaveType
        {
            LeaveTypeCode = model.LeaveTypeCode,
            LeaveTypeName = model.LeaveTypeName,
            IsPaid = model.IsPaid,
            MaxDaysPerYear = model.MaxDaysPerYear,
            CarryForward = model.CarryForward,
            IsActive = model.IsActive,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.LeaveTypes.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Leave Type created successfully.", entity.LeaveTypeID);
    }

    public async Task<(bool Success, string Message)> UpdateLeaveTypeAsync(LeaveTypeViewModel model, int userId)
    {
        var entity = await _db.LeaveTypes.FindAsync(model.LeaveTypeID);
        if (entity == null) return (false, "Leave Type not found.");

        if (await _db.LeaveTypes.AnyAsync(t => t.LeaveTypeCode == model.LeaveTypeCode && t.LeaveTypeID != model.LeaveTypeID))
            return (false, "Leave Type Code already exists.");

        entity.LeaveTypeCode = model.LeaveTypeCode;
        entity.LeaveTypeName = model.LeaveTypeName;
        entity.IsPaid = model.IsPaid;
        entity.MaxDaysPerYear = model.MaxDaysPerYear;
        entity.CarryForward = model.CarryForward;
        entity.IsActive = model.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Leave Type updated successfully.");
    }

    public async Task<(bool Success, string Message)> ToggleLeaveTypeActiveAsync(int id, int userId)
    {
        var entity = await _db.LeaveTypes.FindAsync(id);
        if (entity == null) return (false, "Leave Type not found.");

        entity.IsActive = !entity.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, entity.IsActive ? "Leave Type activated." : "Leave Type deactivated.");
    }

    // --- Leave Requests ---
    public async Task<List<LeaveRequestViewModel>> GetAllLeaveRequestsAsync(string? status = null)
    {
        var query = _db.LeaveRequests.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status == status);

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new LeaveRequestViewModel
            {
                LeaveRequestID = r.LeaveRequestID,
                RequestNo = r.RequestNo,
                EmployeeID = r.EmployeeID,
                EmployeeName = _db.Employees.Where(e => e.EmployeeID == r.EmployeeID).Select(e => e.FirstName + " " + e.LastName).FirstOrDefault(),
                LeaveTypeID = r.LeaveTypeID,
                LeaveTypeName = _db.LeaveTypes.Where(t => t.LeaveTypeID == r.LeaveTypeID).Select(t => t.LeaveTypeName).FirstOrDefault(),
                FromDate = r.FromDate,
                ToDate = r.ToDate,
                TotalDays = r.TotalDays,
                Reason = r.Reason,
                Status = r.Status,
                ApprovedAt = r.ApprovedAt,
                RejectionReason = r.RejectionReason
            }).ToListAsync();
    }

    public async Task<LeaveRequestViewModel?> GetLeaveRequestByIdAsync(int id)
    {
        var r = await _db.LeaveRequests.AsNoTracking().FirstOrDefaultAsync(x => x.LeaveRequestID == id);
        if (r == null) return null;

        return new LeaveRequestViewModel
        {
            LeaveRequestID = r.LeaveRequestID,
            RequestNo = r.RequestNo,
            EmployeeID = r.EmployeeID,
            LeaveTypeID = r.LeaveTypeID,
            FromDate = r.FromDate,
            ToDate = r.ToDate,
            TotalDays = r.TotalDays,
            Reason = r.Reason,
            Status = r.Status,
            ApprovedByUserID = r.ApprovedByUserID,
            ApprovedAt = r.ApprovedAt,
            RejectionReason = r.RejectionReason
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateLeaveRequestAsync(LeaveRequestViewModel model, int userId)
    {
        var lastReq = await _db.LeaveRequests.OrderByDescending(r => r.LeaveRequestID).Select(r => r.RequestNo).FirstOrDefaultAsync();
        var nextNum = 1;
        if (!string.IsNullOrEmpty(lastReq) && lastReq.StartsWith("LR-"))
        {
            if (int.TryParse(lastReq.Replace("LR-", ""), out var n)) nextNum = n + 1;
        }

        var entity = new LeaveRequest
        {
            RequestNo = $"LR-{nextNum:D5}",
            EmployeeID = model.EmployeeID,
            LeaveTypeID = model.LeaveTypeID,
            FromDate = model.FromDate,
            ToDate = model.ToDate,
            TotalDays = model.TotalDays,
            Reason = model.Reason,
            Status = "Pending",
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.LeaveRequests.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Leave Request submitted.", entity.LeaveRequestID);
    }

    public async Task<(bool Success, string Message)> ApproveLeaveRequestAsync(int id, int userId, bool approve, string? rejectionReason)
    {
        var entity = await _db.LeaveRequests.FindAsync(id);
        if (entity == null) return (false, "Leave Request not found.");
        if (entity.Status != "Pending") return (false, "Only pending requests can be approved/rejected.");

        entity.Status = approve ? "Approved" : "Rejected";
        entity.ApprovedByUserID = userId;
        entity.ApprovedAt = DateTime.UtcNow;

        // FIX: 'reject' variable hata diya
        if (!approve)
            entity.RejectionReason = rejectionReason;

        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        // Agar approve hua, toh LeaveBalance update karein
        if (approve)
        {
            var balance = await _db.LeaveBalances
                .FirstOrDefaultAsync(b => b.EmployeeID == entity.EmployeeID && b.LeaveTypeID == entity.LeaveTypeID && b.Year == entity.FromDate.Year);

            if (balance != null)
            {
                balance.UsedDays += entity.TotalDays;
                // FIX: RemainingDays ko manually set nahi karenge, kyunke woh Computed column hai. Database khud calculate karega.
            }
        }

        await _db.SaveChangesAsync();
        return (true, approve ? "Leave Request Approved." : "Leave Request Rejected.");
    }
}