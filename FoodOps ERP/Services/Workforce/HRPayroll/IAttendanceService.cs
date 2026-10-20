using FoodOpsERP.Models.Workforce.HRPayroll.ViewModels;

namespace FoodOpsERP.Services.Workforce.HRPayroll;

public interface IAttendanceService
{
    // Attendance Logs
    Task<List<AttendanceViewModel>> GetAllAttendanceAsync(DateTime? date = null);
    Task<AttendanceViewModel?> GetAttendanceByIdAsync(long id);
    Task<(bool Success, string Message, long? Id)> CreateAttendanceAsync(AttendanceViewModel model, int userId);
    Task<(bool Success, string Message)> UpdateAttendanceAsync(AttendanceViewModel model, int userId);

    // Attendance Corrections
    Task<List<AttendanceCorrectionViewModel>> GetAllCorrectionsAsync(string? status = null);
    Task<AttendanceCorrectionViewModel?> GetCorrectionByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> CreateCorrectionAsync(AttendanceCorrectionViewModel model, int userId);
    Task<(bool Success, string Message)> ApproveCorrectionAsync(int id, int userId, bool approve);
}