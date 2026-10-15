using FoodOpsERP.Models.ViewModels.Inventory;

namespace FoodOpsERP.Services.Inventory
{
    public interface IIncomingQCService
    {
        Task<List<LotViewModel>> GetPendingQCAsync();
        Task<bool> PassQCAsync(long batchId, int inspectorUserId, string? remarks = null);
        Task<bool> FailQCAsync(long batchId, int inspectorUserId, string remarks);
        Task<bool> HoldBatchAsync(long batchId, int userId, string reason);
        Task<bool> ReleaseBatchAsync(long batchId, int userId, string? remarks = null);
    }
}