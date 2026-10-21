using FoodOpsERP.Data;
using FoodOpsERP.Models.AI.Entities;
using FoodOpsERP.Models.AI.ViewModels;

namespace FoodOpsERP.Services.AI;

public class AIAssistantService : IAIAssistantService
{
    private readonly ApplicationDbContext _db;

    public AIAssistantService(ApplicationDbContext db) => _db = db;

    public async Task<string> AskAssistantAsync(string domainContext, string question, int userId)
    {
        // Dummy Response (Baad mein OpenAI API call yahan aayegi)
        var answer = $"Hello! I am your {domainContext} Assistant. You asked: '{question}'. " +
                     "Currently, I am in demo mode. Soon, I will be connected to OpenAI to give you real insights.";

        // Log the AI Call (AIAssistantLogs table mein save hoga)
        var log = new AIAssistantLog
        {
            UserID = userId,
            DomainContext = domainContext,
            QuestionSummary = question.Length > 500 ? question.Substring(0, 500) : question,
            AnswerSummary = answer.Length > 1000 ? answer.Substring(0, 1000) : answer,
            ModelName = "Dummy-Model",
            TokensUsed = 0,
            ExecutionTimeMs = 0,
            IsSuccess = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.AIAssistantLogs.Add(log);
        await _db.SaveChangesAsync();

        return answer;
    }
}