using FoodOpsERP.Data;
using FoodOpsERP.Models.Workforce.HRPayroll.Entities;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public class AttendanceService : IAttendanceService
{
    private readonly ApplicationDbContext _db;

    public AttendanceService(ApplicationDbContext db) => _db = db;

    // --- Attendance Logs ---
    public async Task<List<AttendanceViewModel>> GetAllAttendanceAsync(DateTime? date = null)
    {
        var query = _db.AttendanceLogs.AsNoTracking().AsQueryable();

        if (date.HasValue)
            query = query.Where(a => a.LogDate.Date == date.Value.Date);

        return await query
            .OrderByDescending(a => a.LogDate)
            .Select(a => new AttendanceViewModel
            {
                AttendanceID = a.AttendanceID,
                EmployeeID = a.EmployeeID,
                EmployeeName = _db.Employees.Where(e => e.EmployeeID == a.EmployeeID).Select(e => e.FirstName + " " + e.LastName).FirstOrDefault(),
                ShiftID = a.ShiftID,
                ShiftName = _db.Shifts.Where(s => s.ShiftID == a.ShiftID).Select(s => s.ShiftName).FirstOrDefault(),
                LogDate = a.LogDate,
                CheckInTime = a.CheckInTime,
                CheckOutTime = a.CheckOutTime,
                Status = a.Status,
                WorkedMinutes = a.WorkedMinutes,
                Remarks = a.Remarks
            }).ToListAsync();
    }

    public async Task<AttendanceViewModel?> GetAttendanceByIdAsync(long id)
    {
        var a = await _db.AttendanceLogs.AsNoTracking().FirstOrDefaultAsync(x => x.AttendanceID == id);
        if (a == null) return null;

        return new AttendanceViewModel
        {
            AttendanceID = a.AttendanceID,
            EmployeeID = a.EmployeeID,
            ShiftID = a.ShiftID,
            LogDate = a.LogDate,
            CheckInTime = a.CheckInTime,
            CheckOutTime = a.CheckOutTime,
            Status = a.Status,
            WorkedMinutes = a.WorkedMinutes,
            Remarks = a.Remarks
        };
    }

    public async Task<(bool Success, string Message, long? Id)> CreateAttendanceAsync(AttendanceViewModel model, int userId)
    {
        if (await _db.AttendanceLogs.AnyAsync(a => a.EmployeeID == model.EmployeeID && a.LogDate.Date == model.LogDate.Date))
            return (false, "Attendance for this employee on this date already exists.", null);

        // Calculate Worked Minutes
        int? workedMinutes = null;
        if (model.CheckInTime.HasValue && model.CheckOutTime.HasValue)
        {
            var diff = model.CheckOutTime.Value - model.CheckInTime.Value;
            workedMinutes = (int)diff.TotalMinutes;
        }

        var entity = new AttendanceLog
        {
            EmployeeID = model.EmployeeID,
            ShiftID = model.ShiftID,
            LogDate = model.LogDate,
            CheckInTime = model.CheckInTime,
            CheckOutTime = model.CheckOutTime,
            Status = model.Status,
            WorkedMinutes = workedMinutes,
            Remarks = model.Remarks
        };

        _db.AttendanceLogs.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Attendance recorded successfully.", entity.AttendanceID);
    }

    public async Task<(bool Success, string Message)> UpdateAttendanceAsync(AttendanceViewModel model, int userId)
    {
        var entity = await _db.AttendanceLogs.FindAsync(model.AttendanceID);
        if (entity == null) return (false, "Attendance record not found.");

        int? workedMinutes = null;
        if (model.CheckInTime.HasValue && model.CheckOutTime.HasValue)
        {
            var diff = model.CheckOutTime.Value - model.CheckInTime.Value;
            workedMinutes = (int)diff.TotalMinutes;
        }

        entity.CheckInTime = model.CheckInTime;
        entity.CheckOutTime = model.CheckOutTime;
        entity.Status = model.Status;
        entity.WorkedMinutes = workedMinutes;
        entity.Remarks = model.Remarks;

        await _db.SaveChangesAsync();
        return (true, "Attendance updated successfully.");
    }

    // --- Attendance Corrections ---
    public async Task<List<AttendanceCorrectionViewModel>> GetAllCorrectionsAsync(string? status = null)
    {
        var query = _db.AttendanceCorrections.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(c => c.Status == status);

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new AttendanceCorrectionViewModel
            {
                CorrectionID = c.CorrectionID,
                AttendanceID = c.AttendanceID,
                EmployeeName = _db.AttendanceLogs.Where(a => a.AttendanceID == c.AttendanceID).Select(a => _db.Employees.Where(e => e.EmployeeID == a.EmployeeID).Select(e => e.FirstName + " " + e.LastName).FirstOrDefault()).FirstOrDefault(),
                LogDate = _db.AttendanceLogs.Where(a => a.AttendanceID == c.AttendanceID).Select(a => a.LogDate).FirstOrDefault(),
                OldCheckIn = c.OldCheckIn,
                OldCheckOut = c.OldCheckOut,
                NewCheckIn = c.NewCheckIn,
                NewCheckOut = c.NewCheckOut,
                Reason = c.Reason,
                Status = c.Status,
                ApprovedAt = c.ApprovedAt,
                CreatedAt = c.CreatedAt
            }).ToListAsync();
    }

    public async Task<AttendanceCorrectionViewModel?> GetCorrectionByIdAsync(int id)
    {
        var c = await _db.AttendanceCorrections.AsNoTracking().FirstOrDefaultAsync(x => x.CorrectionID == id);
        if (c == null) return null;

        return new AttendanceCorrectionViewModel
        {
            CorrectionID = c.CorrectionID,
            AttendanceID = c.AttendanceID,
            OldCheckIn = c.OldCheckIn,
            OldCheckOut = c.OldCheckOut,
            NewCheckIn = c.NewCheckIn,
            NewCheckOut = c.NewCheckOut,
            Reason = c.Reason,
            Status = c.Status,
            ApprovedByUserID = c.ApprovedByUserID,
            ApprovedAt = c.ApprovedAt,
            CreatedAt = c.CreatedAt
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateCorrectionAsync(AttendanceCorrectionViewModel model, int userId)
    {
        var attendance = await _db.AttendanceLogs.FindAsync(model.AttendanceID);
        if (attendance == null) return (false, "Attendance record not found.", null);

        var entity = new AttendanceCorrection
        {
            AttendanceID = model.AttendanceID,
            RequestedByUserID = userId,
            OldCheckIn = attendance.CheckInTime,
            OldCheckOut = attendance.CheckOutTime,
            NewCheckIn = model.NewCheckIn,
            NewCheckOut = model.NewCheckOut,
            Reason = model.Reason,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _db.AttendanceCorrections.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Correction request submitted.", entity.CorrectionID);
    }

    public async Task<(bool Success, string Message)> ApproveCorrectionAsync(int id, int userId, bool approve)
    {
        var entity = await _db.AttendanceCorrections.FindAsync(id);
        if (entity == null) return (false, "Correction not found.");
        if (entity.Status != "Pending") return (false, "Only pending corrections can be approved.");

        entity.Status = approve ? "Approved" : "Rejected";
        entity.ApprovedByUserID = userId;
        entity.ApprovedAt = DateTime.UtcNow;

        // Agar approve hua, toh AttendanceLog update karein
        if (approve)
        {
            var attendance = await _db.AttendanceLogs.FindAsync(entity.AttendanceID);
            if (attendance != null)
            {
                attendance.CheckInTime = entity.NewCheckIn;
                attendance.CheckOutTime = entity.NewCheckOut;

                if (attendance.CheckInTime.HasValue && attendance.CheckOutTime.HasValue)
                {
                    var diff = attendance.CheckOutTime.Value - attendance.CheckInTime.Value;
                    attendance.WorkedMinutes = (int)diff.TotalMinutes;
                }
            }
        }

        await _db.SaveChangesAsync();
        return (true, approve ? "Correction approved." : "Correction rejected.");
    }
}