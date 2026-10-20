using FoodOpsERP.Data;
using FoodOpsERP.Models.Workforce.HRPayroll.Entities;
using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public class PayrollService : IPayrollService
{
    private readonly ApplicationDbContext _db;

    public PayrollService(ApplicationDbContext db) => _db = db;

    public async Task<List<PayrollViewModel>> GetAllAsync()
    {
        return await _db.PayrollRuns.AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PayrollViewModel
            {
                PayrollRunID = p.PayrollRunID,
                RunNo = p.RunNo,
                PlantID = p.PlantID,
                PlantName = _db.Plants.Where(pl => pl.PlantID == p.PlantID).Select(pl => pl.PlantName).FirstOrDefault(),
                PeriodMonth = p.PeriodMonth,
                PeriodYear = p.PeriodYear,
                Status = p.Status,
                TotalEmployees = p.TotalEmployees,
                NetPayable = p.NetPayable,
                CalculatedAt = p.CalculatedAt,
                CreatedAt = p.CreatedAt
            }).ToListAsync();
    }

    public async Task<PayrollViewModel?> GetByIdAsync(int id)
    {
        var p = await _db.PayrollRuns.AsNoTracking()
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.PayrollRunID == id);

        if (p == null) return null;

        return new PayrollViewModel
        {
            PayrollRunID = p.PayrollRunID,
            RunNo = p.RunNo,
            PlantID = p.PlantID,
            PeriodMonth = p.PeriodMonth,
            PeriodYear = p.PeriodYear,
            Status = p.Status,
            TotalEmployees = p.TotalEmployees,
            TotalBasicPay = p.TotalBasicPay,
            TotalAllowances = p.TotalAllowances,
            TotalOvertimePay = p.TotalOvertimePay,
            TotalDeductions = p.TotalDeductions,
            NetPayable = p.NetPayable,
            CalculatedAt = p.CalculatedAt,
            CreatedAt = p.CreatedAt,
            Lines = p.Lines.Select(l => new PayrollLineViewModel
            {
                PayrollLineID = l.PayrollLineID,
                PayrollRunID = l.PayrollRunID,
                EmployeeID = l.EmployeeID,
                EmployeeName = _db.Employees.Where(e => e.EmployeeID == l.EmployeeID).Select(e => e.FirstName + " " + e.LastName).FirstOrDefault(),
                BasicPay = l.BasicPay,
                HousingAllowance = l.HousingAllowance,
                TransportAllowance = l.TransportAllowance,
                OtherAllowance = l.OtherAllowance,
                OvertimePay = l.OvertimePay,
                LeaveDeduction = l.LeaveDeduction,
                OtherDeduction = l.OtherDeduction,
                GrossPay = l.GrossPay,
                TotalDeductions = l.TotalDeductions,
                NetPay = l.NetPay,
                WorkingDays = l.WorkingDays,
                PresentDays = l.PresentDays,
                OTHours = l.OTHours,
                Remarks = l.Remarks
            }).ToList()
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateRunAsync(int plantId, int month, int year, int userId)
    {
        if (await _db.PayrollRuns.AnyAsync(p => p.PlantID == plantId && p.PeriodMonth == month && p.PeriodYear == year))
            return (false, "Payroll for this month/year already exists for this plant.", null);

        var lastCode = await _db.PayrollRuns.OrderByDescending(p => p.PayrollRunID).Select(p => p.RunNo).FirstOrDefaultAsync();
        var nextNum = 1;
        if (!string.IsNullOrEmpty(lastCode) && lastCode.StartsWith("PR-"))
        {
            if (int.TryParse(lastCode.Replace("PR-", ""), out var n)) nextNum = n + 1;
        }

        var entity = new PayrollRun
        {
            RunNo = $"PR-{nextNum:D5}",
            PlantID = plantId,
            PeriodMonth = month,
            PeriodYear = year,
            Status = "Draft",
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.PayrollRuns.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Payroll Run created successfully.", entity.PayrollRunID);
    }

    public async Task<(bool Success, string Message)> CalculatePayrollAsync(int payrollRunId, int userId)
    {
        var run = await _db.PayrollRuns.Include(p => p.Lines).FirstOrDefaultAsync(p => p.PayrollRunID == payrollRunId);
        if (run == null) return (false, "Payroll Run not found.");
        if (run.Status != "Draft") return (false, "Only Draft payrolls can be calculated.");

        // Delete old lines if recalculating
        if (run.Lines.Any()) _db.PayrollLines.RemoveRange(run.Lines);

        // Get all active employees for this plant
        var employees = await _db.Employees.AsNoTracking()
            .Where(e => e.PlantID == run.PlantID && e.IsActive)
            .ToListAsync();

        decimal totalBasic = 0, totalAllow = 0, totalOT = 0, totalDed = 0, totalNet = 0;

        foreach (var emp in employees)
        {
            // Get current salary
            var salary = await _db.EmployeeSalaries.AsNoTracking()
                .Where(s => s.EmployeeID == emp.EmployeeID && s.Status == "Active")
                .OrderByDescending(s => s.EffectiveFrom)
                .FirstOrDefaultAsync();

            if (salary == null) continue; // Skip if no salary setup

            // Get attendance for the month
            var startDate = new DateTime(run.PeriodYear, run.PeriodMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var attendanceLogs = await _db.AttendanceLogs.AsNoTracking()
                .Where(a => a.EmployeeID == emp.EmployeeID && a.LogDate >= startDate && a.LogDate <= endDate)
                .ToListAsync();

            int workingDays = 26; // Standard
            int presentDays = attendanceLogs.Count(a => a.Status == "Present" || a.Status == "Late");

            // Get approved overtime
            var overtime = await _db.OvertimeRecords.AsNoTracking()
                .Where(o => o.EmployeeID == emp.EmployeeID && o.OTDate >= startDate && o.OTDate <= endDate && o.Status == "Approved")
                .SumAsync(o => o.OTHours);

            // Calculate OT Pay (Simple formula: (Basic / 26 / 8) * 1.5 * Hours)
            decimal hourlyRate = salary.BasicSalary / workingDays / 8;
            decimal otPay = overtime * hourlyRate * 1.5m;

            // Calculate deductions (e.g., absent days)
            decimal absentDeduction = (workingDays - presentDays) * (salary.BasicSalary / workingDays);
            if (absentDeduction < 0) absentDeduction = 0;

            decimal allowances = salary.HousingAllowance + salary.TransportAllowance + salary.OtherAllowance;
            decimal gross = salary.BasicSalary + allowances + otPay;
            decimal net = gross - absentDeduction;

            var line = new PayrollLine
            {
                PayrollRunID = run.PayrollRunID,
                EmployeeID = emp.EmployeeID,
                BasicPay = salary.BasicSalary,
                HousingAllowance = salary.HousingAllowance,
                TransportAllowance = salary.TransportAllowance,
                OtherAllowance = salary.OtherAllowance,
                OvertimePay = otPay,
                LeaveDeduction = absentDeduction,
                OtherDeduction = 0,
                GrossPay = gross,
                TotalDeductions = absentDeduction,
                NetPay = net,
                WorkingDays = workingDays,
                PresentDays = presentDays,
                OTHours = overtime
            };

            _db.PayrollLines.Add(line);

            totalBasic += salary.BasicSalary;
            totalAllow += allowances;
            totalOT += otPay;
            totalDed += absentDeduction;
            totalNet += net;
        }

        run.TotalEmployees = employees.Count;
        run.TotalBasicPay = totalBasic;
        run.TotalAllowances = totalAllow;
        run.TotalOvertimePay = totalOT;
        run.TotalDeductions = totalDed;
        run.NetPayable = totalNet;
        run.Status = "Calculated";
        run.CalculatedAt = DateTime.UtcNow;
        run.ModifiedByUserID = userId;
        run.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Payroll calculated successfully.");
    }

    public async Task<(bool Success, string Message)> ApprovePayrollAsync(int payrollRunId, int userId, string comments)
    {
        var run = await _db.PayrollRuns.FindAsync(payrollRunId);
        if (run == null) return (false, "Payroll Run not found.");
        if (run.Status != "Calculated" && run.Status != "UnderReview")
            return (false, "Only Calculated payrolls can be approved.");

        var approval = new PayrollApproval
        {
            PayrollRunID = payrollRunId,
            StepNo = 1, // Assuming single step for now
            ApproverUserID = userId,
            Decision = "Approved",
            Comments = comments,
            DecidedAt = DateTime.UtcNow
        };
        _db.PayrollApprovals.Add(approval);

        run.Status = "Approved";
        run.ModifiedByUserID = userId;
        run.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Payroll approved successfully.");
    }

    public async Task<(bool Success, string Message)> FinalizePayrollAsync(int payrollRunId, int userId)
    {
        var run = await _db.PayrollRuns.FindAsync(payrollRunId);
        if (run == null) return (false, "Payroll Run not found.");
        if (run.Status != "Approved") return (false, "Only Approved payrolls can be finalized.");

        run.Status = "Paid";
        run.ModifiedByUserID = userId;
        run.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Payroll finalized and marked as Paid.");
    }
}