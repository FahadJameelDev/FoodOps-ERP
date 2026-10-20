using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public interface IPayrollService
{
    Task<List<PayrollViewModel>> GetAllAsync();
    Task<PayrollViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateRunAsync(int plantId, int month, int year, int userId);
    Task<(bool Success, string Message)> CalculatePayrollAsync(int payrollRunId, int userId);
    Task<(bool Success, string Message)> ApprovePayrollAsync(int payrollRunId, int userId, string comments);
    Task<(bool Success, string Message)> FinalizePayrollAsync(int payrollRunId, int userId);
}