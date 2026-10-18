using FoodOpsERP.Models.Manufacturing.Production.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.Production;

public interface IDemandService
{
    Task<List<DemandViewModel>> GetAllAsync(string? status = null, int? plantId = null);
    Task<DemandViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateAsync(DemandViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateAsync(DemandViewModel model, int userId);
    Task<(bool Success, string Message)> CancelAsync(int id, int userId);
}