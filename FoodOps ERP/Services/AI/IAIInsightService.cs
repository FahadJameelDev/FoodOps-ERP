using FoodOpsERP.Models.AI.ViewModels;

namespace FoodOpsERP.Services.AI;

public interface IAIInsightService
{
    Task<AIInsightCacheViewModel?> GetCacheAsync(string moduleContext, string cacheKey);
    Task SaveCacheAsync(string moduleContext, string cacheKey, string payloadJson, int expiryMinutes);
    Task ClearExpiredCacheAsync();
}