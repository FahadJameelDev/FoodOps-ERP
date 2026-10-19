using FoodOpsERP.Data;
using FoodOpsERP.Models.Manufacturing.Costing.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.Costing;

public class BatchCostingService : IBatchCostingService
{
    private readonly ApplicationDbContext _db;

    public BatchCostingService(ApplicationDbContext db) => _db = db;

    public async Task<List<BatchCostSummaryViewModel>> GetAllBatchCostsAsync()
    {
        return await _db.BatchCostSummaries.AsNoTracking()
            .OrderByDescending(x => x.CalculatedAt)
            .Select(x => new BatchCostSummaryViewModel
            {
                BatchCostID = x.BatchCostID,
                ProductionOrderID = x.ProductionOrderID,
                OrderNumber = _db.ProductionOrders.Where(o => o.ProductionOrderID == x.ProductionOrderID).Select(o => o.OrderNumber).FirstOrDefault(),
                ActualMaterialCost = x.ActualMaterialCost,
                ActualLaborCost = x.ActualLaborCost,
                ActualOverheadCost = x.ActualOverheadCost,
                TotalBatchCost = x.TotalBatchCost,
                GoodQty = x.GoodQty,
                UnitCost = x.UnitCost,
                StandardUnitCostSnapshot = x.StandardUnitCostSnapshot,
                Status = x.Status,
                CalculatedAt = x.CalculatedAt
            }).ToListAsync();
    }

    public async Task<List<BatchCostSummaryViewModel>> GetUnitCostsAsync()
    {
        var list = await GetAllBatchCostsAsync();
        return list.Where(x => x.UnitCost.HasValue).ToList();
    }

    public async Task<List<CostVarianceViewModel>> GetAllVariancesAsync()
    {
        return await _db.CostVariances.AsNoTracking()
            .OrderByDescending(x => x.CalculatedAt)
            .Select(x => new CostVarianceViewModel
            {
                VarianceID = x.VarianceID,
                ProductionOrderID = x.ProductionOrderID,
                OrderNumber = _db.ProductionOrders.Where(o => o.ProductionOrderID == x.ProductionOrderID).Select(o => o.OrderNumber).FirstOrDefault(),
                MaterialPriceVariance = x.MaterialPriceVariance,
                MaterialUsageVariance = x.MaterialUsageVariance,
                LaborVariance = x.LaborVariance,
                OverheadVariance = x.OverheadVariance,
                YieldVariance = x.YieldVariance,
                TotalVarianceAmount = x.TotalVarianceAmount,
                TotalVariancePercent = x.TotalVariancePercent,
                Severity = x.Severity,
                ReviewStatus = x.ReviewStatus,
                ReviewComments = x.ReviewComments,
                CalculatedAt = x.CalculatedAt
            }).ToListAsync();
    }

    public async Task<List<CostVarianceViewModel>> GetVarianceControlsAsync()
    {
        var list = await GetAllVariancesAsync();
        return list.Where(x => x.ReviewStatus != "NotRequired" && x.ReviewStatus != "Closed").ToList();
    }

    public async Task<CostVarianceViewModel?> GetVarianceDetailsAsync(int id)
    {
        var x = await _db.CostVariances.AsNoTracking().FirstOrDefaultAsync(v => v.VarianceID == id);
        if (x == null) return null;

        return new CostVarianceViewModel
        {
            VarianceID = x.VarianceID,
            ProductionOrderID = x.ProductionOrderID,
            OrderNumber = await _db.ProductionOrders.Where(o => o.ProductionOrderID == x.ProductionOrderID).Select(o => o.OrderNumber).FirstOrDefaultAsync(),
            MaterialPriceVariance = x.MaterialPriceVariance,
            MaterialUsageVariance = x.MaterialUsageVariance,
            LaborVariance = x.LaborVariance,
            OverheadVariance = x.OverheadVariance,
            YieldVariance = x.YieldVariance,
            TotalVarianceAmount = x.TotalVarianceAmount,
            TotalVariancePercent = x.TotalVariancePercent,
            Severity = x.Severity,
            ReviewStatus = x.ReviewStatus,
            ReviewComments = x.ReviewComments,
            CalculatedAt = x.CalculatedAt
        };
    }

    public async Task<(bool Success, string Message)> ReviewVarianceAsync(int id, string status, string comments, int userId)
    {
        var entity = await _db.CostVariances.FindAsync(id);
        if (entity == null) return (false, "Variance record not found.");

        entity.ReviewStatus = status;
        entity.ReviewComments = comments;
        entity.ReviewedByUserID = userId;
        entity.ReviewedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Variance review updated.");
    }

    public async Task<object> GetLossAnalysisAsync()
    {
        // 1. Total Wastage Qty (ProductionWastages table se)
        var totalWastageQty = await _db.ProductionWastages.SumAsync(w => w.Quantity);

        // 2. Total Downtime Hours (DowntimeLogs table se)
        var totalDowntimeMinutes = await _db.DowntimeLogs
            .Where(d => d.DurationMinutes.HasValue)
            .SumAsync(d => d.DurationMinutes.Value);
        var totalDowntimeHours = totalDowntimeMinutes / 60.0m;

        // 3. Details List (Top 10 Wastage Records)
        var wastageDetails = await _db.ProductionWastages.AsNoTracking()
            .OrderByDescending(w => w.RecordedAt)
            .Take(10)
            .Select(w => new
            {
                Date = w.RecordedAt.ToString("dd MMM yyyy"),
                Type = w.WastageType,
                Quantity = w.Quantity,
                Reason = w.Reason
            }).ToListAsync();

        return new
        {
            TotalWastageQty = totalWastageQty,
            TotalDowntimeHours = totalDowntimeHours,
            WastageDetails = wastageDetails
        };
    }
}