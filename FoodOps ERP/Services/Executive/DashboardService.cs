using FoodOpsERP.Data;
using FoodOpsERP.Models.Executive.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Executive;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _db;

    public DashboardService(ApplicationDbContext db) => _db = db;

    public async Task<DashboardViewModel> GetDashboardDataAsync()
    {
        var model = new DashboardViewModel();
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var thisMonth = new DateTime(today.Year, today.Month, 1);

        // 1. Factory Overview
        model.TotalEmployees = await _db.Employees.CountAsync(e => e.IsActive);
        model.TotalProducts = await _db.Products.CountAsync(p => p.IsActive);

        // ✅ FIX 1: Date translation fix
        model.TodayProductionQty = await _db.ProductionOrders
            .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow)
            .SumAsync(o => o.ProducedQty);

        model.TotalInventoryValue = await _db.InventoryBatches.SumAsync(b => b.CurrentQuantity);

        // 2. Production KPIs
        model.ActiveProductionOrders = await _db.ProductionOrders
            .CountAsync(o => o.Status == "InProgress" || o.Status == "Released" || o.Status == "Planned");
        model.TodayProducedQty = model.TodayProductionQty;
        model.TodayRejectedQty = await _db.ProductionOrders
            .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow)
            .SumAsync(o => o.RejectedQty);

        // 3. Inventory KPIs
        model.LowStockItemsCount = await _db.InventoryBatches.CountAsync(b => b.CurrentQuantity > 0 && b.CurrentQuantity < 10);
        model.OutOfStockItemsCount = await _db.InventoryBatches.CountAsync(b => b.CurrentQuantity <= 0);
        model.ExpiringLotsCount = await _db.InventoryBatches
            .CountAsync(b => b.ExpiryDate.HasValue && b.ExpiryDate.Value <= today.AddDays(30) && b.ExpiryDate.Value >= today);

        // 4. Quality KPIs (✅ FIX 2: Real Data)
        model.PendingQCInspections = await _db.QCInspections.CountAsync(q => q.OverallResult == null || q.OverallResult == "Pending");
        model.QCOnHoldBatches = await _db.InventoryBatches.CountAsync(b => b.QCStatus == "Hold" || b.QCStatus == "Quarantine");

        var totalInspections = await _db.QCInspections.CountAsync();
        var passedInspections = await _db.QCInspections.CountAsync(q => q.OverallResult == "Pass");
        model.QualityPassRate = totalInspections > 0 ? (decimal)passedInspections / totalInspections * 100 : 0;

        // 5. Cost KPIs (✅ FIX 3: Column Fix)
        model.TotalBatchCostThisMonth = await _db.BatchCostSummaries
            .Where(b => b.CalculatedAt >= thisMonth)
            .SumAsync(b => b.TotalBatchCost);

        // CostVariances table mein CalculatedAt nahi hai, ProductionOrder ke through aayenge
        model.TotalVarianceThisMonth = await _db.CostVariances
            .SumAsync(v => v.TotalVarianceAmount);

        // 6. Workforce KPIs
        var totalEmp = await _db.Employees.CountAsync(e => e.IsActive);
        var todayLogs = await _db.AttendanceLogs
            .Where(a => a.LogDate >= today && a.LogDate < tomorrow && (a.Status == "Present" || a.Status == "Late"))
            .ToListAsync();

        model.TodayPresentEmployees = todayLogs.Count;
        model.TodayAbsentEmployees = totalEmp - model.TodayPresentEmployees;
        model.AttendancePercentage = totalEmp > 0 ? (decimal)model.TodayPresentEmployees / totalEmp * 100 : 0;
        model.PendingLeaveRequests = await _db.LeaveRequests.CountAsync(l => l.Status == "Pending");

        // 7. Critical Alerts
        if (model.LowStockItemsCount > 0)
            model.CriticalAlerts.Add(new AlertItem { Title = "Low Stock", Description = $"{model.LowStockItemsCount} items are below reorder level.", Severity = "Warning", Link = "/Stock/Balance" });

        if (model.QCOnHoldBatches > 0)
            model.CriticalAlerts.Add(new AlertItem { Title = "QC Hold", Description = $"{model.QCOnHoldBatches} batches are on hold.", Severity = "Critical", Link = "/QualityDecisions" });

        if (model.ExpiringLotsCount > 0)
            model.CriticalAlerts.Add(new AlertItem { Title = "Expiring Lots", Description = $"{model.ExpiringLotsCount} lots are expiring in 30 days.", Severity = "Warning", Link = "/Lots/Expiry" });

        // 8. Action Queue
        if (model.QCOnHoldBatches > 0)
            model.ActionQueue.Add(new ActionItem { Task = $"Release {model.QCOnHoldBatches} held batches", Module = "Quality", Link = "/QualityDecisions" });

        if (model.PendingLeaveRequests > 0)
            model.ActionQueue.Add(new ActionItem { Task = $"Review {model.PendingLeaveRequests} leave requests", Module = "HR", Link = "/Leave" });

        // 9. Trends (Last 7 Days)
        for (int i = 6; i >= 0; i--)
        {
            var d = today.AddDays(-i);
            var nextD = d.AddDays(1);
            var qty = await _db.ProductionOrders
                .Where(o => o.CreatedAt >= d && o.CreatedAt < nextD)
                .SumAsync(o => o.ProducedQty);
            model.ProductionTrend.Add(new ChartDataPoint { Label = d.ToString("ddd"), Value = qty });
        }

        // 10. AI Summary
        model.AiSummary = $"Today's production is {model.TodayProducedQty:N0} units. Attendance is {model.AttendancePercentage:N1}%.";

        return model;
    }
}