using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Manufacturing.Production.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.Production;

public class ProductionOrderService : IProductionOrderService
{
    private readonly ApplicationDbContext _db;

    public ProductionOrderService(ApplicationDbContext db) => _db = db;

    public async Task<List<ProductionOrderViewModel>> GetAllAsync(string? status = null, int? plantId = null)
    {
        var query = _db.ProductionOrders.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(o => o.Status == status);

        if (plantId.HasValue)
            query = query.Where(o => o.PlantID == plantId.Value);

        return await query
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new ProductionOrderViewModel
            {
                ProductionOrderID = o.ProductionOrderID,
                OrderNumber = o.OrderNumber,
                PlantID = o.PlantID,
                PlantName = _db.Plants.Where(p => p.PlantID == o.PlantID).Select(p => p.PlantName).FirstOrDefault(),
                ProductID = o.ProductID,
                ProductName = _db.Products.Where(p => p.ProductID == o.ProductID).Select(p => p.ProductName).FirstOrDefault(),
                RecipeID = o.RecipeID,
                RecipeCode = _db.Recipes.Where(r => r.RecipeID == o.RecipeID).Select(r => r.RecipeCode).FirstOrDefault(),
                TargetQty = o.TargetQty,
                ProducedQty = o.ProducedQty,
                RejectedQty = o.RejectedQty,
                UOMID = o.UOMID,
                UOMName = _db.UnitsOfMeasure.Where(u => u.UOMID == o.UOMID).Select(u => u.UOMName).FirstOrDefault(),
                Priority = o.Priority,
                PlannedStart = o.PlannedStart,
                PlannedEnd = o.PlannedEnd,
                ActualStart = o.ActualStart,
                ActualEnd = o.ActualEnd,
                Status = o.Status,
                Remarks = o.Remarks,
                CreatedAt = o.CreatedAt
            }).ToListAsync();
    }

    public async Task<ProductionOrderViewModel?> GetByIdAsync(int id)
    {
        var o = await _db.ProductionOrders.AsNoTracking().FirstOrDefaultAsync(x => x.ProductionOrderID == id);
        if (o == null) return null;

        var operations = await _db.ProductionOperations.AsNoTracking()
            .Where(op => op.ProductionOrderID == id)
            .OrderBy(op => op.SequenceOrder)
            .Select(op => new ProductionOperationViewModel
            {
                OperationID = op.OperationID,
                ProductionOrderID = op.ProductionOrderID,
                RoutingID = op.RoutingID,
                WorkCenterID = op.WorkCenterID,
                WorkCenterName = _db.WorkCenters.Where(w => w.WorkCenterID == op.WorkCenterID).Select(w => w.WorkCenterName).FirstOrDefault(),
                SequenceOrder = op.SequenceOrder,
                OperationName = op.OperationName,
                StandardMinutes = op.StandardMinutes,
                Status = op.Status,
                StartTime = op.StartTime,
                EndTime = op.EndTime,
                ActualMinutes = op.ActualMinutes,
                OperatorUserID = op.OperatorUserID
            }).ToListAsync();

        return new ProductionOrderViewModel
        {
            ProductionOrderID = o.ProductionOrderID,
            OrderNumber = o.OrderNumber,
            PlantID = o.PlantID,
            PlanLineID = o.PlanLineID,
            ProductID = o.ProductID,
            ProductName = await _db.Products.Where(p => p.ProductID == o.ProductID).Select(p => p.ProductName).FirstOrDefaultAsync(),
            RecipeID = o.RecipeID,
            RecipeCode = await _db.Recipes.Where(r => r.RecipeID == o.RecipeID).Select(r => r.RecipeCode).FirstOrDefaultAsync(),
            TargetQty = o.TargetQty,
            ProducedQty = o.ProducedQty,
            RejectedQty = o.RejectedQty,
            UOMID = o.UOMID,
            Priority = o.Priority,
            PlannedStart = o.PlannedStart,
            PlannedEnd = o.PlannedEnd,
            ActualStart = o.ActualStart,
            ActualEnd = o.ActualEnd,
            Status = o.Status,
            Remarks = o.Remarks,
            CreatedAt = o.CreatedAt,
            Operations = operations
        };
    }

    public async Task<ProductionExecutionViewModel?> GetExecutionAsync(int id)
    {
        var order = await GetByIdAsync(id);
        if (order == null) return null;

        return new ProductionExecutionViewModel
        {
            Order = order,
            Operations = order.Operations
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(ProductionOrderViewModel model, int userId)
    {
        if (model.PlannedEnd < model.PlannedStart)
            return (false, "Planned End cannot be before Planned Start.", null);

        var lastCode = await _db.ProductionOrders.OrderByDescending(o => o.ProductionOrderID).Select(o => o.OrderNumber).FirstOrDefaultAsync();
        var nextNumber = 1;
        if (!string.IsNullOrEmpty(lastCode) && lastCode.StartsWith("PO-"))
        {
            if (int.TryParse(lastCode.Replace("PO-", ""), out var n)) nextNumber = n + 1;
        }

        var entity = new ProductionOrder
        {
            OrderNumber = $"PO-{nextNumber:D5}",
            PlantID = model.PlantID,
            PlanLineID = model.PlanLineID,
            ProductID = model.ProductID,
            RecipeID = model.RecipeID,
            TargetQty = model.TargetQty,
            UOMID = model.UOMID,
            Priority = model.Priority,
            PlannedStart = model.PlannedStart,
            PlannedEnd = model.PlannedEnd,
            Status = "Planned",
            Remarks = model.Remarks,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.ProductionOrders.Add(entity);
        await _db.SaveChangesAsync();

        // Auto-create operations from routing
        var routings = await _db.ProductionRoutings
            .Where(r => r.ProductID == model.ProductID && r.IsActive)
            .OrderBy(r => r.SequenceOrder)
            .ToListAsync();

        foreach (var route in routings)
        {
            _db.ProductionOperations.Add(new ProductionOperation
            {
                ProductionOrderID = entity.ProductionOrderID,
                RoutingID = route.RoutingID,
                WorkCenterID = route.WorkCenterID,
                SequenceOrder = route.SequenceOrder,
                OperationName = route.OperationName,
                StandardMinutes = route.StandardMinutes,
                Status = "Pending",
                CreatedByUserID = userId,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync();
        return (true, "Production Order created successfully.", entity.ProductionOrderID);
    }

    public async Task<(bool Success, string Message)> ReleaseAsync(int orderId, int userId)
    {
        var e = await _db.ProductionOrders.FindAsync(orderId);
        if (e == null || e.Status != "Planned") return (false, "Invalid order or status.");

        e.Status = "Released";
        e.ModifiedByUserID = userId; e.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return (true, "Order released.");
    }

    public async Task<(bool Success, string Message)> StartAsync(int orderId, int userId)
    {
        var e = await _db.ProductionOrders.FindAsync(orderId);
        if (e == null || (e.Status != "Released" && e.Status != "OnHold")) return (false, "Invalid order or status.");

        e.Status = "InProgress";
        e.ActualStart ??= DateTime.UtcNow;
        e.ModifiedByUserID = userId; e.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return (true, "Order started.");
    }

    public async Task<(bool Success, string Message)> CompleteAsync(int orderId, int userId)
    {
        var e = await _db.ProductionOrders.FindAsync(orderId);
        if (e == null || e.Status != "InProgress") return (false, "Invalid order or status.");

        e.Status = "Completed";
        e.ActualEnd = DateTime.UtcNow;
        e.ModifiedByUserID = userId; e.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return (true, "Order completed.");
    }

    public async Task<(bool Success, string Message)> CloseAsync(int orderId, int userId)
    {
        var e = await _db.ProductionOrders.FindAsync(orderId);
        if (e == null || e.Status != "Completed") return (false, "Invalid order or status.");

        e.Status = "Closed";
        e.ModifiedByUserID = userId; e.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return (true, "Order closed.");
    }

    public async Task<(bool Success, string Message)> HoldAsync(int orderId, int userId)
    {
        var e = await _db.ProductionOrders.FindAsync(orderId);
        if (e == null || e.Status != "InProgress") return (false, "Invalid order or status.");

        e.Status = "OnHold";
        e.ModifiedByUserID = userId; e.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return (true, "Order put on hold.");
    }

    public async Task<(bool Success, string Message)> CancelAsync(int orderId, int userId)
    {
        var e = await _db.ProductionOrders.FindAsync(orderId);
        if (e == null || e.Status == "Closed" || e.Status == "Cancelled") return (false, "Invalid order or status.");

        e.Status = "Cancelled";
        e.ModifiedByUserID = userId; e.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return (true, "Order cancelled.");
    }

    public async Task<(bool Success, string Message)> StartOperationAsync(int operationId, int userId)
    {
        var op = await _db.ProductionOperations.FindAsync(operationId);
        if (op == null || (op.Status != "Pending" && op.Status != "Paused")) return (false, "Invalid operation.");

        op.Status = "Running";
        op.StartTime ??= DateTime.UtcNow;
        op.OperatorUserID = userId;
        op.ModifiedByUserID = userId; op.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return (true, "Operation started.");
    }

    public async Task<(bool Success, string Message)> CompleteOperationAsync(int operationId, int userId)
    {
        var op = await _db.ProductionOperations.FindAsync(operationId);
        if (op == null || op.Status != "Running") return (false, "Invalid operation.");

        op.Status = "Completed";
        op.EndTime = DateTime.UtcNow;
        op.ModifiedByUserID = userId; op.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return (true, "Operation completed.");
    }

    public async Task<(bool Success, string Message)> RecordWastageAsync(int orderId, string wastageType, decimal qty, string reason, int userId)
    {
        var order = await _db.ProductionOrders.FindAsync(orderId);
        if (order == null) return (false, "Order not found.");

        var wastage = new ProductionWastage
        {
            ProductionOrderID = orderId,
            ProductID = order.ProductID,
            WastageType = wastageType,
            Quantity = qty,
            UOMID = order.UOMID,
            Reason = reason,
            RecordedByUserID = userId,
            RecordedAt = DateTime.UtcNow,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };
        _db.ProductionWastages.Add(wastage);
        await _db.SaveChangesAsync();
        return (true, "Wastage recorded.");
    }

    public async Task<(bool Success, string Message)> RecordExceptionAsync(int orderId, string exceptionType, string severity, string description, int userId)
    {
        var lastCode = await _db.ProductionExceptions.OrderByDescending(o => o.ExceptionID).Select(o => o.ExceptionNo).FirstOrDefaultAsync();
        var nextNumber = 1;
        if (!string.IsNullOrEmpty(lastCode) && lastCode.StartsWith("EXC-"))
        {
            if (int.TryParse(lastCode.Replace("EXC-", ""), out var n)) nextNumber = n + 1;
        }

        var ex = new ProductionException
        {
            ExceptionNo = $"EXC-{nextNumber:D5}",
            ProductionOrderID = orderId,
            ExceptionType = exceptionType,
            Severity = severity,
            Description = description,
            Status = "Open",
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };
        _db.ProductionExceptions.Add(ex);
        await _db.SaveChangesAsync();
        return (true, "Exception recorded.");
    }
}