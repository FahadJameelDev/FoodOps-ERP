using FoodOpsERP.Data;
using FoodOpsERP.Models.Executive.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Executive;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _db;

    public ReportService(ApplicationDbContext db) => _db = db;

    public async Task<ReportResultViewModel> GenerateReportAsync(ReportFilterViewModel filter)
    {
        var result = new ReportResultViewModel
        {
            ReportTitle = $"{filter.ReportType} Report",
            ReportType = filter.ReportType,
            StartDate = filter.StartDate,
            EndDate = filter.EndDate
        };

        switch (filter.ReportType)
        {
            case "Production":
                await GenerateProductionReport(result, filter);
                break;
            case "Inventory":
                await GenerateInventoryReport(result, filter);
                break;
            case "Quality":
                await GenerateQualityReport(result, filter);
                break;
            case "Workforce":
                await GenerateWorkforceReport(result, filter);
                break;
            default:
                result.Headers.Add("Message");
                result.Rows.Add(new[] { "Report type not implemented yet." });
                break;
        }

        return result;
    }

    private async Task GenerateProductionReport(ReportResultViewModel result, ReportFilterViewModel filter)
    {
        result.Headers.AddRange(new[] { "Order #", "Product", "Target Qty", "Produced Qty", "Rejected Qty", "Date", "Status" });

        var query = _db.ProductionOrders.AsNoTracking()
            .Where(o => o.CreatedAt.Date >= filter.StartDate.Date && o.CreatedAt.Date <= filter.EndDate.Date);

        if (filter.PlantID.HasValue)
            query = query.Where(o => o.PlantID == filter.PlantID.Value);

        var data = await query.OrderByDescending(o => o.CreatedAt).Take(500).ToListAsync();

        foreach (var item in data)
        {
            result.Rows.Add(new[]
            {
                item.OrderNumber,
                item.ProductID.ToString(), // Baad mein ProductName se replace karein
                item.TargetQty.ToString("N2"),
                item.ProducedQty.ToString("N2"),
                item.RejectedQty.ToString("N2"),
                item.CreatedAt.ToString("dd MMM yyyy"),
                item.Status
            });
        }
    }

    private async Task GenerateInventoryReport(ReportResultViewModel result, ReportFilterViewModel filter)
    {
        result.Headers.AddRange(new[] { "Batch #", "Product ID", "Quantity", "Expiry", "QC Status" });

        var data = await _db.InventoryBatches.AsNoTracking()
            .Where(b => b.CreatedAt.Date >= filter.StartDate.Date && b.CreatedAt.Date <= filter.EndDate.Date)
            .OrderByDescending(b => b.CreatedAt).Take(500).ToListAsync();

        foreach (var item in data)
        {
            result.Rows.Add(new[]
            {
                item.BatchNumber,
                item.ProductID.ToString(),
                item.CurrentQuantity.ToString("N2"),
                item.ExpiryDate?.ToString("dd MMM yyyy") ?? "—",
                item.QCStatus
            });
        }
    }

    private async Task GenerateQualityReport(ReportResultViewModel result, ReportFilterViewModel filter)
    {
        result.Headers.AddRange(new[] { "Inspection ID", "Batch ID", "Date", "Overall Result" });

        // NOTE: Purani QC table mein Status/Result ke naam alag hain, isliye simple list
        var data = await _db.QCInspections.AsNoTracking()
            .Where(q => q.InspectionDate >= filter.StartDate.Date && q.InspectionDate <= filter.EndDate.Date)
            .OrderByDescending(q => q.InspectionDate).Take(500).ToListAsync();

        foreach (var item in data)
        {
            result.Rows.Add(new[]
            {
                item.InspectionID.ToString(),
                item.BatchID.ToString(),
                item.InspectionDate.ToString("dd MMM yyyy"),
                "—" // Placeholder
            });
        }
    }

    private async Task GenerateWorkforceReport(ReportResultViewModel result, ReportFilterViewModel filter)
    {
        result.Headers.AddRange(new[] { "Employee", "Date", "Check In", "Check Out", "Status" });

        var data = await _db.AttendanceLogs.AsNoTracking()
            .Where(a => a.LogDate.Date >= filter.StartDate.Date && a.LogDate.Date <= filter.EndDate.Date)
            .OrderByDescending(a => a.LogDate).Take(500).ToListAsync();

        foreach (var item in data)
        {
            result.Rows.Add(new[]
            {
                item.EmployeeID.ToString(),
                item.LogDate.ToString("dd MMM yyyy"),
                item.CheckInTime?.ToString("HH:mm") ?? "—",
                item.CheckOutTime?.ToString("HH:mm") ?? "—",
                item.Status
            });
        }
    }
}