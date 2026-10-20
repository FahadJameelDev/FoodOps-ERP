using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public interface IWorkforceSetupService
{
    // Departments
    Task<List<DepartmentViewModel>> GetAllDepartmentsAsync();
    Task<DepartmentViewModel?> GetDepartmentByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateDepartmentAsync(DepartmentViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateDepartmentAsync(DepartmentViewModel model, int userId);
    Task<(bool Success, string Message)> ToggleDepartmentActiveAsync(int id, int userId);

    // Shifts
    Task<List<ShiftViewModel>> GetAllShiftsAsync();
    Task<ShiftViewModel?> GetShiftByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateShiftAsync(ShiftViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateShiftAsync(ShiftViewModel model, int userId);
    Task<(bool Success, string Message)> ToggleShiftActiveAsync(int id, int userId);
}