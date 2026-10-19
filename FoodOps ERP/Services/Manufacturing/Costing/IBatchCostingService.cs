using FoodOpsERP.Models.Manufacturing.Costing.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.Costing;

public interface IBatchCostingService
{
    Task<List<BatchCostSummaryViewModel>> GetAllBatchCostsAsync();
    Task<List<BatchCostSummaryViewModel>> GetUnitCostsAsync();
    Task<List<CostVarianceViewModel>> GetAllVariancesAsync();
    Task<List<CostVarianceViewModel>> GetVarianceControlsAsync();
    Task<CostVarianceViewModel?> GetVarianceDetailsAsync(int id);
    Task<(bool Success, string Message)> ReviewVarianceAsync(int id, string status, string comments, int userId);
    Task<object> GetLossAnalysisAsync();
}