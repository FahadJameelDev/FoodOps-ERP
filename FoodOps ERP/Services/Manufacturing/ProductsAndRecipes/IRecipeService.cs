using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;

public interface IRecipeService
{
    Task<List<RecipeViewModel>> GetAllAsync(string? status = null);
    Task<RecipeViewModel?> GetByIdAsync(int id);
    Task<List<RecipeViewModel>> GetPendingApprovalsAsync();
    Task<(bool Success, string Message, int? Id)> CreateAsync(RecipeViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateAsync(RecipeViewModel model, int userId);
    Task<(bool Success, string Message)> SubmitForApprovalAsync(int recipeId, int userId);
    Task<(bool Success, string Message)> ApproveAsync(int recipeId, int approverId, string? comments);
    Task<(bool Success, string Message)> RejectAsync(int recipeId, int approverId, string comments);
    Task<(bool Success, string Message)> SetCurrentAsync(int recipeId, int userId);
}