using FoodOpsERP.Models.Manufacturing.Production.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.Production;

public interface IProductionPlanService
{
    Task<List<ProductionPlanViewModel>> GetAllAsync(string? status = null, int? plantId = null);
    Task<ProductionPlanViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateAsync(ProductionPlanViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateAsync(ProductionPlanViewModel model, int userId);
    Task<(bool Success, string Message)> ApproveAsync(int planId, int userId);
    Task<(bool Success, string Message)> ReleaseAsync(int planId, int userId);
    Task<(bool Success, string Message)> CloseAsync(int planId, int userId);
    Task<(bool Success, string Message)> CancelAsync(int planId, int userId);
}