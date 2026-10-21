using FoodOpsERP.Data;
using FoodOpsERP.Models.AI.Entities;
using FoodOpsERP.Models.AI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.AI;

public class AIInsightService : IAIInsightService
{
    private readonly ApplicationDbContext _db;

    public AIInsightService(ApplicationDbContext db) => _db = db;

    public async Task<AIInsightCacheViewModel?> GetCacheAsync(string moduleContext, string cacheKey)
    {
        var cache = await _db.AIInsightCaches.AsNoTracking()
            .FirstOrDefaultAsync(c => c.ModuleContext == moduleContext && c.CacheKey == cacheKey);

        if (cache == null || cache.ExpiresAt < DateTime.UtcNow)
            return null; // Cache expired or not found

        return new AIInsightCacheViewModel
        {
            CacheID = cache.CacheID,
            ModuleContext = cache.ModuleContext,
            CacheKey = cache.CacheKey,
            PayloadJson = cache.PayloadJson,
            GeneratedAt = cache.GeneratedAt,
            ExpiresAt = cache.ExpiresAt
        };
    }

    public async Task SaveCacheAsync(string moduleContext, string cacheKey, string payloadJson, int expiryMinutes)
    {
        var existing = await _db.AIInsightCaches
            .FirstOrDefaultAsync(c => c.ModuleContext == moduleContext && c.CacheKey == cacheKey);

        if (existing != null)
        {
            existing.PayloadJson = payloadJson;
            existing.GeneratedAt = DateTime.UtcNow;
            existing.ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
        }
        else
        {
            _db.AIInsightCaches.Add(new AIInsightCache
            {
                ModuleContext = moduleContext,
                CacheKey = cacheKey,
                PayloadJson = payloadJson,
                GeneratedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes)
            });
        }

        await _db.SaveChangesAsync();
    }

    public async Task ClearExpiredCacheAsync()
    {
        var expired = await _db.AIInsightCaches.Where(c => c.ExpiresAt < DateTime.UtcNow).ToListAsync();
        if (expired.Any())
        {
            _db.AIInsightCaches.RemoveRange(expired);
            await _db.SaveChangesAsync();
        }
    }
}