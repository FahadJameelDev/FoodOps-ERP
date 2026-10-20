using FoodOpsERP.Models.Security.ViewModels;

namespace FoodOpsERP.Services.Security;

public interface IRoleService
{
    Task<List<RoleViewModel>> GetAllRolesAsync();
    Task<RoleViewModel?> GetRoleByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateRoleAsync(RoleViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateRoleAsync(RoleViewModel model, int userId);
    Task<RolePermissionViewModel?> GetRolePermissionsAsync(int roleId);
    Task<(bool Success, string Message)> SaveRolePermissionsAsync(int roleId, List<int> permissionIds, int userId);
}