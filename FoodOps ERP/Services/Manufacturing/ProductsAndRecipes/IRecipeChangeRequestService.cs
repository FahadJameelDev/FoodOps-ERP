using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;

public interface IRecipeChangeRequestService
{
    Task<List<RecipeChangeRequestViewModel>> GetAllAsync(int? recipeId = null);
    Task<RecipeChangeRequestViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateAsync(RecipeChangeRequestViewModel model, int userId);
    Task<(bool Success, string Message)> ApproveAsync(int id, int approverId, string comments);
    Task<(bool Success, string Message)> RejectAsync(int id, int approverId, string comments);
}