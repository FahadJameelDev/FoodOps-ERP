using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.AI.ViewModels;

public class AIChatViewModel
{
    [Required(ErrorMessage = "Please enter your question.")]
    [StringLength(500)]
    public string Question { get; set; } = string.Empty;

    public string DomainContext { get; set; } = string.Empty; // Inventory, Production, etc.

    public string? Answer { get; set; }
    public List<ChatMessage> History { get; set; } = new();
}

public class ChatMessage
{
    public string Role { get; set; } = "user"; // user or assistant
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}