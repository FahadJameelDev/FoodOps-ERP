using FoodOpsERP.Data;
using FoodOpsERP.Models.AI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.AI;

public class AIAuditService : IAIAuditService
{
    private readonly ApplicationDbContext _db;

    public AIAuditService(ApplicationDbContext db) => _db = db;

    public async Task<List<AIAssistantLogViewModel>> GetAllLogsAsync(string? domain = null)
    {
        var query = _db.AIAssistantLogs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(domain))
            query = query.Where(l => l.DomainContext == domain);

        return await query
            .OrderByDescending(l => l.CreatedAt)
            .Take(500)
            .Select(l => new AIAssistantLogViewModel
            {
                AILogID = l.AILogID,
                UserID = l.UserID,
                UserName = _db.Users.Where(u => u.UserID == l.UserID).Select(u => u.Username).FirstOrDefault(),
                DomainContext = l.DomainContext,
                QuestionSummary = l.QuestionSummary,
                AnswerSummary = l.AnswerSummary,
                ModelName = l.ModelName,
                TokensUsed = l.TokensUsed,
                ExecutionTimeMs = l.ExecutionTimeMs,
                IsSuccess = l.IsSuccess,
                ErrorMessage = l.ErrorMessage,
                CreatedAt = l.CreatedAt
            }).ToListAsync();
    }

    public async Task<AIAssistantLogViewModel?> GetLogByIdAsync(long id)
    {
        var l = await _db.AIAssistantLogs.AsNoTracking().FirstOrDefaultAsync(x => x.AILogID == id);
        if (l == null) return null;

        return new AIAssistantLogViewModel
        {
            AILogID = l.AILogID,
            UserID = l.UserID,
            DomainContext = l.DomainContext,
            QuestionSummary = l.QuestionSummary,
            AnswerSummary = l.AnswerSummary,
            ModelName = l.ModelName,
            TokensUsed = l.TokensUsed,
            ExecutionTimeMs = l.ExecutionTimeMs,
            IsSuccess = l.IsSuccess,
            ErrorMessage = l.ErrorMessage,
            CreatedAt = l.CreatedAt
        };
    }

    public async Task<List<AIAssistantLogViewModel>> GetEvidenceLogsAsync()
    {
        // Filhal Evidence same as Logs hai
        return await GetAllLogsAsync();
    }
}