using FoodOpsERP.Models.Manufacturing.Costing.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.Costing;

public interface ICostSetupService
{
    // Components
    Task<List<CostComponentViewModel>> GetAllComponentsAsync(bool? activeOnly = null);
    Task<CostComponentViewModel?> GetComponentByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateComponentAsync(CostComponentViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateComponentAsync(CostComponentViewModel model, int userId);
    Task<(bool Success, string Message)> ToggleComponentActiveAsync(int id, int userId);

    // Standard Costs
    Task<List<StandardCostViewModel>> GetAllStandardCostsAsync();
    Task<StandardCostViewModel?> GetStandardCostByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateStandardCostAsync(StandardCostViewModel model, int userId);
    Task<(bool Success, string Message)> ApproveStandardCostAsync(int id, int userId);
}