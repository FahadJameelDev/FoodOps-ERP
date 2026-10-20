using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public interface ILeaveService
{
    // Leave Types
    Task<List<LeaveTypeViewModel>> GetAllLeaveTypesAsync();
    Task<LeaveTypeViewModel?> GetLeaveTypeByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateLeaveTypeAsync(LeaveTypeViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateLeaveTypeAsync(LeaveTypeViewModel model, int userId);
    Task<(bool Success, string Message)> ToggleLeaveTypeActiveAsync(int id, int userId);

    // Leave Requests
    Task<List<LeaveRequestViewModel>> GetAllLeaveRequestsAsync(string? status = null);
    Task<LeaveRequestViewModel?> GetLeaveRequestByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateLeaveRequestAsync(LeaveRequestViewModel model, int userId);
    Task<(bool Success, string Message)> ApproveLeaveRequestAsync(int id, int userId, bool approve, string? rejectionReason);
}