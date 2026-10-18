using FoodOpsERP.Models.Manufacturing.Quality.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.Quality;

public interface IQCParameterService
{
    Task<List<QCParameterViewModel>> GetAllAsync(bool? activeOnly = null);
    Task<QCParameterViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateAsync(QCParameterViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateAsync(QCParameterViewModel model, int userId);
    Task<(bool Success, string Message)> ToggleActiveAsync(int id, int userId);
}