using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public interface IOvertimeService
{
    Task<List<OvertimeViewModel>> GetAllAsync(string? status = null);
    Task<OvertimeViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateAsync(OvertimeViewModel model, int userId);
    Task<(bool Success, string Message)> ApproveAsync(int id, int userId, bool approve);
}