using FoodOpsERP.Services.Inventory;
using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.ViewModels.Inventory;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Inventory
{
    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _context;

        public StockService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockBalanceViewModel>> GetStockBalanceAsync()
        {
            return await _context.InventoryBatches
                .AsNoTracking()
                .Include(x => x.Product)
                    .ThenInclude(p => p.BaseUOM)
                .Where(x => x.CurrentQuantity > 0) // ✅ FIX: Sirf available stock
                .GroupBy(x => new
                {
                    x.ProductID,
                    x.Product.ProductCode,
                    x.Product.ProductName,
                    x.Product.MinStockLevel,
                    UOM = x.Product.BaseUOM.UOMCode
                })
                .Select(g => new StockBalanceViewModel
                {
                    ProductID = g.Key.ProductID,
                    ProductCode = g.Key.ProductCode,
                    ProductName = g.Key.ProductName,
                    UOM = g.Key.UOM,
                    TotalQuantity = g.Sum(x => x.CurrentQuantity),
                    MinStockLevel = g.Key.MinStockLevel ?? 0,
                    // ✅ FIX: Stock status logic
                    StockStatus = g.Sum(x => x.CurrentQuantity) <= 0 ? "Out"
                                : g.Sum(x => x.CurrentQuantity) <= (g.Key.MinStockLevel ?? 0) ? "Low"
                                : "OK"
                })
                .OrderBy(x => x.ProductName)
                .ToListAsync();
        }

        public async Task<List<LotViewModel>> GetLotsAsync()
        {
            return await _context.InventoryBatches
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.Supplier)
                .Include(x => x.WarehouseBin)
                .Where(x => x.CurrentQuantity > 0)
                .OrderBy(x => x.ExpiryDate)
                .Select(x => new LotViewModel
                {
                    BatchID = x.BatchID,
                    BatchNumber = x.BatchNumber,
                    ProductName = x.Product.ProductName,
                    SupplierName = x.Supplier != null ? x.Supplier.SupplierName : null,
                    CurrentQuantity = x.CurrentQuantity,
                    ExpiryDate = x.ExpiryDate,
                    QCStatus = x.QCStatus,
                    BinCode = x.WarehouseBin != null ? x.WarehouseBin.BinCode : null,
                    // ✅ FIX: Safe DaysToExpiry
                    DaysToExpiry = x.ExpiryDate.HasValue
                        ? EF.Functions.DateDiffDay(DateTime.Today, x.ExpiryDate.Value)
                        : 999
                }).ToListAsync();
        }

        public async Task<List<LotViewModel>> GetExpiringLotsAsync(int days = 30)
        {
            var threshold = DateTime.Today.AddDays(days);

            return await _context.InventoryBatches
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.Supplier)
                .Where(x => x.CurrentQuantity > 0
                         && x.ExpiryDate != null
                         && x.ExpiryDate <= threshold)
                .OrderBy(x => x.ExpiryDate)
                .Select(x => new LotViewModel
                {
                    BatchID = x.BatchID,
                    BatchNumber = x.BatchNumber,
                    ProductName = x.Product.ProductName,
                    SupplierName = x.Supplier != null ? x.Supplier.SupplierName : null,
                    CurrentQuantity = x.CurrentQuantity,
                    ExpiryDate = x.ExpiryDate,
                    QCStatus = x.QCStatus,
                    // ✅ FIX: Safe DaysToExpiry
                    DaysToExpiry = x.ExpiryDate.HasValue
                        ? EF.Functions.DateDiffDay(DateTime.Today, x.ExpiryDate.Value)
                        : 0
                }).ToListAsync();
        }

        public async Task<bool> AdjustStockAsync(long batchId, decimal qty, string reason, int userId, string? remarks = null)
        {
            // ✅ FIX: Transaction wrap
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var batch = await _context.InventoryBatches.FindAsync(batchId);
                if (batch == null) return false;

                // ✅ FIX: Zero adjustment reject
                if (qty == 0)
                    throw new InvalidOperationException("Adjustment quantity cannot be zero.");

                // ✅ FIX: Negative stock check
                var newQty = batch.CurrentQuantity + qty;
                if (newQty < 0)
                    throw new InvalidOperationException(
                        $"Cannot adjust stock. Current: {batch.CurrentQuantity}, Adjustment: {qty}, Result would be: {newQty}. Stock cannot be negative.");

                batch.CurrentQuantity = newQty;
                batch.ModifiedAt = DateTime.UtcNow;

                _context.StockAdjustments.Add(new StockAdjustment
                {
                    BatchID = batchId,
                    AdjustedQty = qty,
                    Reason = reason,
                    Remarks = remarks,
                    AdjustedByUserID = userId,
                    AdjustedAt = DateTime.UtcNow
                });

                _context.InventoryTransactions.Add(new InventoryTransaction
                {
                    BatchID = batchId,
                    PlantID = batch.PlantID,
                    TransactionType = "Adjustment",
                    Quantity = qty,
                    ReferenceType = "Adjustment",
                    Remarks = remarks,
                    CreatedByUserID = userId,
                    Timestamp = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}