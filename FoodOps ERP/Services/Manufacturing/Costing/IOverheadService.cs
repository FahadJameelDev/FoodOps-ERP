using FoodOpsERP.Models.Manufacturing.Costing.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.Costing;

public interface IOverheadService
{
    Task<List<OverheadAllocationViewModel>> GetAllAsync();
    Task<OverheadAllocationViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateAsync(OverheadAllocationViewModel model, int userId);
    Task<(bool Success, string Message)> PostAsync(int id, int userId);
}