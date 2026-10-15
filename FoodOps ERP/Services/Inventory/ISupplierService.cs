using FoodOpsERP.Models.ViewModels.Inventory;

namespace FoodOpsERP.Services.Inventory
{
    public interface ISupplierService
    {
        Task<List<SupplierViewModel>> GetAllAsync();
        Task<SupplierViewModel?> GetByIdAsync(int id);
        Task<bool> CreateAsync(SupplierViewModel model);
        Task<bool> UpdateAsync(SupplierViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}