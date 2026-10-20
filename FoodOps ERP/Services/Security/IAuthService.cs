using FoodOpsERP.Models.Security.ViewModels;

namespace FoodOpsERP.Services.Security;

public interface IAuthService
{
    Task<(bool Success, string Message, int? UserId)> ValidateLoginAsync(LoginViewModel model);
    Task<List<string>> GetUserRolesAsync(int userId);
    Task<List<string>> GetUserPermissionsAsync(int userId);
}