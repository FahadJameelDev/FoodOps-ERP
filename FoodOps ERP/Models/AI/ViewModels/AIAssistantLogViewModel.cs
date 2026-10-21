namespace FoodOpsERP.Models.AI.ViewModels;

public class AIAssistantLogViewModel
{
    public long AILogID { get; set; }
    public int UserID { get; set; }
    public string? UserName { get; set; }
    public string DomainContext { get; set; } = string.Empty;
    public string QuestionSummary { get; set; } = string.Empty;
    public string? AnswerSummary { get; set; }
    public string? ModelName { get; set; }
    public int? TokensUsed { get; set; }
    public int? ExecutionTimeMs { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
}