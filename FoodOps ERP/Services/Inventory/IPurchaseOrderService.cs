using FoodOpsERP.Models.ViewModels.Inventory;

namespace FoodOpsERP.Services.Inventory
{
    public interface IPurchaseOrderService
    {
        Task<List<PurchaseOrderViewModel>> GetAllAsync();
        Task<PurchaseOrderViewModel?> GetByIdAsync(int id);
        Task<bool> CreateAsync(PurchaseOrderViewModel model);
        Task<bool> UpdateAsync(PurchaseOrderViewModel model);
        Task<bool> ApproveAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}