using FoodOpsERP.Models.AI.ViewModels;

namespace FoodOpsERP.Services.AI;

public interface IAIAuditService
{
    Task<List<AIAssistantLogViewModel>> GetAllLogsAsync(string? domain = null);
    Task<AIAssistantLogViewModel?> GetLogByIdAsync(long id);
    Task<List<AIAssistantLogViewModel>> GetEvidenceLogsAsync();
}