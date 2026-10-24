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
            case "Cost":
                await GenerateCostReport(result, filter);
                break;
            case "Workforce":
                await GenerateWorkforceReport(result, filter);
                break;
            case "Executive":
                await GenerateExecutiveReport(result, filter);
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
            .Where(o => o.CreatedAt >= filter.StartDate && o.CreatedAt <= filter.EndDate);

        if (filter.PlantID.HasValue)
            query = query.Where(o => o.PlantID == filter.PlantID.Value);

        var data = await query.OrderByDescending(o => o.CreatedAt).Take(500).ToListAsync();

        foreach (var item in data)
        {
            var productName = await _db.Products.Where(p => p.ProductID == item.ProductID).Select(p => p.ProductName).FirstOrDefaultAsync();
            result.Rows.Add(new[]
            {
                item.OrderNumber ?? "—",
                productName ?? item.ProductID.ToString(),
                item.TargetQty.ToString("N2"),
                item.ProducedQty.ToString("N2"),
                item.RejectedQty.ToString("N2"),
                item.CreatedAt.ToString("dd MMM yyyy"),
                item.Status ?? "—"
            });
        }
    }

    private async Task GenerateInventoryReport(ReportResultViewModel result, ReportFilterViewModel filter)
    {
        result.Headers.AddRange(new[] { "Batch #", "Product", "Quantity", "Expiry Date", "QC Status" });

        var data = await _db.InventoryBatches.AsNoTracking()
            .Where(b => b.CreatedAt >= filter.StartDate && b.CreatedAt <= filter.EndDate)
            .OrderByDescending(b => b.CreatedAt).Take(500).ToListAsync();

        foreach (var item in data)
        {
            var productName = await _db.Products.Where(p => p.ProductID == item.ProductID).Select(p => p.ProductName).FirstOrDefaultAsync();
            result.Rows.Add(new[]
            {
                item.BatchNumber ?? "—",
                productName ?? item.ProductID.ToString(),
                item.CurrentQuantity.ToString("N2"),
                item.ExpiryDate?.ToString("dd MMM yyyy") ?? "—",
                item.QCStatus ?? "Unspecified"
            });
        }
    }

    private async Task GenerateQualityReport(ReportResultViewModel result, ReportFilterViewModel filter)
    {
        result.Headers.AddRange(new[] { "Inspection ID", "Batch ID", "Inspection Date", "Overall Result" });

        var data = await _db.QCInspections.AsNoTracking()
            .Where(q => q.InspectionDate >= filter.StartDate && q.InspectionDate <= filter.EndDate)
            .OrderByDescending(q => q.InspectionDate).Take(500).ToListAsync();

        foreach (var item in data)
        {
            result.Rows.Add(new[]
            {
                item.InspectionID.ToString(),
                item.BatchID != 0 ? item.BatchID.ToString() : "—",
                item.InspectionDate.ToString("dd MMM yyyy"),
                item.OverallResult ?? "Pending"
            });
        }
    }

    private async Task GenerateCostReport(ReportResultViewModel result, ReportFilterViewModel filter)
    {
        result.Headers.AddRange(new[] { "Production Order ID", "Total Batch Cost", "Calculated Date" });

        var data = await _db.BatchCostSummaries.AsNoTracking()
            .Where(b => b.CalculatedAt >= filter.StartDate && b.CalculatedAt <= filter.EndDate)
            .OrderByDescending(b => b.CalculatedAt).Take(500).ToListAsync();

        foreach (var item in data)
        {
            result.Rows.Add(new[]
            {
                item.ProductionOrderID != 0 ? item.ProductionOrderID.ToString() : "—",
                item.TotalBatchCost.ToString("N2"),
                item.CalculatedAt.ToString("dd MMM yyyy")
            });
        }
    }

    private async Task GenerateWorkforceReport(ReportResultViewModel result, ReportFilterViewModel filter)
    {
        result.Headers.AddRange(new[] { "Employee", "Log Date", "Check In", "Check Out", "Status" });

        var data = await _db.AttendanceLogs.AsNoTracking()
            .Where(a => a.LogDate >= filter.StartDate && a.LogDate <= filter.EndDate)
            .OrderByDescending(a => a.LogDate).Take(500).ToListAsync();

        foreach (var item in data)
        {
            var empName = await _db.Employees.Where(e => e.EmployeeID == item.EmployeeID).Select(e => e.FirstName + " " + e.LastName).FirstOrDefaultAsync();
            result.Rows.Add(new[]
            {
                empName ?? item.EmployeeID.ToString(),
                item.LogDate.ToString("dd MMM yyyy"),
                item.CheckInTime?.ToString("HH:mm") ?? "—",
                item.CheckOutTime?.ToString("HH:mm") ?? "—",
                item.Status ?? "—"
            });
        }
    }

    private async Task GenerateExecutiveReport(ReportResultViewModel result, ReportFilterViewModel filter)
    {
        result.Headers.AddRange(new[] { "Metric Name", "Aggregated Value", "Period" });

        var totalProd = await _db.ProductionOrders.Where(p => p.CreatedAt >= filter.StartDate && p.CreatedAt <= filter.EndDate).SumAsync(p => p.ProducedQty);
        var totalCost = await _db.BatchCostSummaries.Where(b => b.CalculatedAt >= filter.StartDate && b.CalculatedAt <= filter.EndDate).SumAsync(b => b.TotalBatchCost);
        var activeEmp = await _db.Employees.CountAsync(e => e.IsActive);
        var qcInspections = await _db.QCInspections.CountAsync(q => q.InspectionDate >= filter.StartDate && q.InspectionDate <= filter.EndDate);

        var periodStr = $"{filter.StartDate:dd MMM yyyy} - {filter.EndDate:dd MMM yyyy}";

        result.Rows.Add(new[] { "Total Production Output (Units)", totalProd.ToString("N2"), periodStr });
        result.Rows.Add(new[] { "Total Manufacturing Cost (PKR)", totalCost.ToString("N2"), periodStr });
        result.Rows.Add(new[] { "Active Workforce Count", activeEmp.ToString(), periodStr });
        result.Rows.Add(new[] { "Quality Inspections Conducted", qcInspections.ToString(), periodStr });
    }
}