using System.ComponentModel.DataAnnotations;

namespace FoodOpsERP.Models.AI.Entities;

public class AIInsightCache
{
    public int CacheID { get; set; }

    [Required, MaxLength(50)]
    public string ModuleContext { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string CacheKey { get; set; } = string.Empty;

    [Required]
    public string PayloadJson { get; set; } = string.Empty;

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
}