using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;

public interface IProductService
{
    Task<List<ProductViewModel>> GetAllAsync();
    Task<ProductViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message)> CreateAsync(ProductViewModel model);
    Task<(bool Success, string Message)> UpdateAsync(ProductViewModel model);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}