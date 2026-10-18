using FoodOpsERP.Models.Manufacturing.Quality.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.Quality;

public interface IQualityDecisionService
{
    Task<List<QualityDecisionViewModel>> GetAllAsync(long? batchId = null);
    Task<QualityDecisionViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateAsync(QualityDecisionViewModel model, int userId);
}