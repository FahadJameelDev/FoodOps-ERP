using FoodOpsERP.Data;
using FoodOpsERP.Models.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Inventory;

public interface IInventoryLookupService
{
    Task<List<SelectListItem>> SuppliersAsync();
    Task<List<SelectListItem>> PlantsAsync();
    Task<List<SelectListItem>> ProductsAsync();
    Task<List<SelectListItem>> UomsAsync();
    Task<List<SelectListItem>> BinsAsync();
    Task<List<SelectListItem>> BatchOptionsAsync(bool inStockOnly);
    Task<BatchRow?> GetBatchAsync(long id);
    Task<List<StockMovementRow>> MovementsAsync(int take = 200);
}

public sealed class InventoryLookupService : IInventoryLookupService
{
    private readonly ApplicationDbContext _db;
    public InventoryLookupService(ApplicationDbContext db) => _db = db;

    // ORDER BY LINQ se (raw SQL ke andar ORDER BY subquery mein invalid hota hai)
    private static async Task<List<SelectListItem>> ToItems(IQueryable<LookupRow> q) =>
        (await q.OrderBy(x => x.Text).ToListAsync())
            .Select(x => new SelectListItem(x.Text, x.Id.ToString())).ToList();

    public Task<List<SelectListItem>> SuppliersAsync() => ToItems(_db.Database.SqlQuery<LookupRow>(
        $"SELECT SupplierID AS [Id], SupplierCode + N' — ' + SupplierName AS [Text] FROM Suppliers WHERE IsActive = 1"));

    public Task<List<SelectListItem>> PlantsAsync() => ToItems(_db.Database.SqlQuery<LookupRow>(
        $"SELECT PlantID AS [Id], PlantName AS [Text] FROM Plants WHERE IsActive = 1"));

    public Task<List<SelectListItem>> ProductsAsync() => ToItems(_db.Database.SqlQuery<LookupRow>(
        $"SELECT ProductID AS [Id], ProductCode + N' — ' + ProductName AS [Text] FROM Products WHERE IsActive = 1"));

    public Task<List<SelectListItem>> UomsAsync() => ToItems(_db.Database.SqlQuery<LookupRow>(
        $"SELECT UOMID AS [Id], UOMCode AS [Text] FROM UnitsOfMeasure WHERE IsActive = 1"));

    public Task<List<SelectListItem>> BinsAsync() => ToItems(_db.Database.SqlQuery<LookupRow>(
        $"SELECT b.BinID AS [Id], w.WarehouseName + N' / ' + b.BinCode AS [Text] FROM WarehouseBins b INNER JOIN Warehouses w ON w.WarehouseID = b.WarehouseID WHERE b.IsActive = 1"));

    private IQueryable<BatchRow> Batches() => _db.Database.SqlQuery<BatchRow>(
        $"SELECT b.BatchID, b.BatchNumber, p.ProductName, b.CurrentQuantity, b.QCStatus FROM InventoryBatches b INNER JOIN Products p ON p.ProductID = b.ProductID");

    public async Task<List<SelectListItem>> BatchOptionsAsync(bool inStockOnly)
    {
        var q = Batches();
        if (inStockOnly) q = q.Where(b => b.CurrentQuantity > 0);
        var rows = await q.OrderBy(b => b.ProductName).ThenBy(b => b.BatchNumber).ToListAsync();
        return rows.Select(b => new SelectListItem(
            $"{b.BatchNumber} · {b.ProductName} · {b.CurrentQuantity:#,##0.##} in stock", b.BatchID.ToString())).ToList();
    }

    public Task<BatchRow?> GetBatchAsync(long id) => Batches().Where(b => b.BatchID == id).FirstOrDefaultAsync();

    public Task<List<StockMovementRow>> MovementsAsync(int take = 200) => _db.Database.SqlQuery<StockMovementRow>(
        $"SELECT t.TransactionID, b.BatchNumber, p.ProductName, t.TransactionType, t.Quantity, t.Remarks, t.[Timestamp] AS [Timestamp] FROM InventoryTransactions t INNER JOIN InventoryBatches b ON b.BatchID = t.BatchID INNER JOIN Products p ON p.ProductID = b.ProductID")
        .OrderByDescending(t => t.Timestamp).Take(take).ToListAsync();
}