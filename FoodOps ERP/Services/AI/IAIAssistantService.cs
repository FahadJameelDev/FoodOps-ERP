using FoodOpsERP.Models.AI.ViewModels;

namespace FoodOpsERP.Services.AI;

public interface IAIAssistantService
{
    // Dummy AI response generator (Baad mein OpenAI se replace hoga)
    Task<string> AskAssistantAsync(string domainContext, string question, int userId);
}