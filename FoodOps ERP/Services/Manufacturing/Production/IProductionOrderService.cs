using FoodOpsERP.Models.Manufacturing.Production.ViewModels;

namespace FoodOpsERP.Services.Manufacturing.Production;

public interface IProductionOrderService
{
    Task<List<ProductionOrderViewModel>> GetAllAsync(string? status = null, int? plantId = null);
    Task<ProductionOrderViewModel?> GetByIdAsync(int id);
    Task<ProductionExecutionViewModel?> GetExecutionAsync(int id);

    Task<(bool Success, string Message, int? Id)> CreateAsync(ProductionOrderViewModel model, int userId);

    // Order Status Workflow
    Task<(bool Success, string Message)> ReleaseAsync(int orderId, int userId);
    Task<(bool Success, string Message)> StartAsync(int orderId, int userId);
    Task<(bool Success, string Message)> CompleteAsync(int orderId, int userId);
    Task<(bool Success, string Message)> CloseAsync(int orderId, int userId);
    Task<(bool Success, string Message)> HoldAsync(int orderId, int userId);
    Task<(bool Success, string Message)> CancelAsync(int orderId, int userId);

    // Operations Workflow
    Task<(bool Success, string Message)> StartOperationAsync(int operationId, int userId);
    Task<(bool Success, string Message)> CompleteOperationAsync(int operationId, int userId);

    // Wastage & Exceptions
    Task<(bool Success, string Message)> RecordWastageAsync(int orderId, string wastageType, decimal qty, string reason, int userId);
    Task<(bool Success, string Message)> RecordExceptionAsync(int orderId, string exceptionType, string severity, string description, int userId);
}