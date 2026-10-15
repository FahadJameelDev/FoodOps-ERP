using FoodOpsERP.Models.ViewModels.Inventory;

namespace FoodOpsERP.Services.Inventory
{
    public interface IGoodsReceivingService
    {
        Task<List<PurchaseOrderViewModel>> GetPendingPOsAsync();
        Task<GRNViewModel?> GetPOForReceivingAsync(int poId);
        Task<bool> CreateGRNAsync(GRNViewModel model, int userId);
    }
}