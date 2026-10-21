namespace FoodOpsERP.Models.AI.ViewModels;

public class AIInsightCacheViewModel
{
    public int CacheID { get; set; }
    public string ModuleContext { get; set; } = string.Empty;
    public string CacheKey { get; set; } = string.Empty;
    public string PayloadJson { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}