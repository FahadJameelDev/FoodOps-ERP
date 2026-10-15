using FoodOpsERP.Models.ViewModels.Inventory;

namespace FoodOpsERP.Services.Inventory
{
    public interface IStockService
    {
        Task<List<StockBalanceViewModel>> GetStockBalanceAsync();
        Task<List<LotViewModel>> GetLotsAsync();
        Task<List<LotViewModel>> GetExpiringLotsAsync(int days = 30);
        Task<bool> AdjustStockAsync(long batchId, decimal qty, string reason, int userId, string? remarks = null);
    }
}