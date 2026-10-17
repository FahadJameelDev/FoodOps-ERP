using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;

public interface IPackagingConfigService
{
    Task<List<PackagingConfigViewModel>> GetAllAsync(int? productId = null);
    Task<PackagingConfigViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateAsync(PackagingConfigViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateAsync(PackagingConfigViewModel model, int userId);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}