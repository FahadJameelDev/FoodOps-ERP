using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public interface IEmployeeService
{
    Task<List<EmployeeViewModel>> GetAllAsync();
    Task<EmployeeViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateAsync(EmployeeViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateAsync(EmployeeViewModel model, int userId);
    Task<(bool Success, string Message)> ToggleActiveAsync(int id, int userId);
}