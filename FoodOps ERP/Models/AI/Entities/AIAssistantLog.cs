using System.ComponentModel.DataAnnotations;
using FoodOpsERP.Models.Entities;

namespace FoodOpsERP.Models.AI.Entities;

public static class AIDomainContexts
{
    public const string Inventory = "Inventory";
    public const string Production = "Production";
    public const string Quality = "Quality";
    public const string Cost = "Cost";
    public const string Payroll = "Payroll";
    public const string Management = "Management";
    public static readonly string[] All =
    {
        Inventory, Production, Quality, Cost, Payroll, Management
    };
}

public class AIAssistantLog
{
    public long AILogID { get; set; }
    public int UserID { get; set; }

    [Required, MaxLength(30)]
    public string DomainContext { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string QuestionSummary { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? AnswerSummary { get; set; }

    [MaxLength(50)]
    public string? ModelName { get; set; }

    public int? TokensUsed { get; set; }
    public int? ExecutionTimeMs { get; set; }

    public bool IsSuccess { get; set; } = true;

    [MaxLength(500)]
    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
}