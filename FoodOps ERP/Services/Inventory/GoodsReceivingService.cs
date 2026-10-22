using FoodOpsERP.Services.Inventory;
using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.ViewModels.Inventory;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Inventory
{
    public class GoodsReceivingService : IGoodsReceivingService
    {
        private readonly ApplicationDbContext _context;

        public GoodsReceivingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseOrderViewModel>> GetPendingPOsAsync()
        {
            return await _context.PurchaseOrders
                .AsNoTracking()
                .Include(x => x.Supplier)
                .Where(x => x.Status == "Approved" || x.Status == "PartiallyReceived")
                .OrderByDescending(x => x.OrderDate)
                .Select(x => new PurchaseOrderViewModel
                {
                    POID = x.POID,
                    POCode = x.POCode,
                    SupplierID = x.SupplierID,
                    SupplierName = x.Supplier.SupplierName,
                    Status = x.Status,
                    OrderDate = x.OrderDate,
                    ExpectedDate = x.ExpectedDate
                }).ToListAsync();
        }

        public async Task<GRNViewModel?> GetPOForReceivingAsync(int poId)
        {
            var po = await _context.PurchaseOrders
                .AsNoTracking()
                .Include(x => x.Supplier)
                .Include(x => x.PurchaseOrderItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.POID == poId);

            if (po == null) return null;

            return new GRNViewModel
            {
                POID = po.POID,
                POCode = po.POCode,
                SupplierID = po.SupplierID,
                SupplierName = po.Supplier.SupplierName,
                ReceivedDate = DateTime.Today,
                Items = po.PurchaseOrderItems.Select(i => new GRNItemViewModel
                {
                    POItemID = i.POItemID,
                    ProductID = i.ProductID,
                    ProductName = i.Product.ProductName,
                    OrderedQty = i.OrderedQty,
                    AlreadyReceivedQty = i.ReceivedQty,
                    ReceivingQty = i.OrderedQty - i.ReceivedQty
                }).ToList()
            };
        }

        public async Task<bool> CreateGRNAsync(GRNViewModel model, int userId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var po = await _context.PurchaseOrders
                        .Include(x => x.PurchaseOrderItems)
                        .FirstOrDefaultAsync(x => x.POID == model.POID);

                    if (po == null)
                        throw new InvalidOperationException($"PO ID {model.POID} not found.");

                    if (po.Status != "Approved" && po.Status != "PartiallyReceived")
                        throw new InvalidOperationException($"Cannot create GRN for PO in '{po.Status}' status.");

                    // ✅ FIX: View se ReceivingQty aa rahi hai
                    var receivingItems = model.Items.Where(x => x.ReceivingQty > 0).ToList();
                    if (!receivingItems.Any())
                        throw new InvalidOperationException("At least one item must have receiving quantity > 0.");

                    // ✅ FIX: Fallback Bins aur Suppliers
                    var defaultBinId = await _context.WarehouseBins.Select(x => (int?)x.BinID).FirstOrDefaultAsync() ?? 1;
                    int validSupplierId = model.SupplierID > 0 ? model.SupplierID : (await _context.Suppliers.Select(s => s.SupplierID).FirstOrDefaultAsync());

                    var batches = new List<InventoryBatch>();

                    foreach (var item in receivingItems)
                    {
                        var poItem = po.PurchaseOrderItems
                            .FirstOrDefault(x => x.POItemID == item.POItemID)
                            ?? throw new InvalidOperationException($"PO Item {item.POItemID} not found.");

                        if (poItem.ReceivedQty + item.ReceivingQty > poItem.OrderedQty)
                            throw new InvalidOperationException(
                                $"Over-receipt for '{item.ProductName}'. Ordered: {poItem.OrderedQty}, Already received: {poItem.ReceivedQty}, Trying to receive: {item.ReceivingQty}");

                        // Agar expiry date nahi di, toh default 1 saal aage laga do
                        DateTime? expDate = item.ExpiryDate ?? DateTime.Today.AddYears(1);
                        if (expDate.Value.Date <= DateTime.Today)
                            throw new InvalidOperationException($"Expiry date for '{item.ProductName}' must be in the future.");

                        poItem.ReceivedQty += item.ReceivingQty;

                        int targetBinId = (item.BinID.HasValue && item.BinID > 0) ? item.BinID.Value : defaultBinId;

                        var batch = new InventoryBatch
                        {
                            BatchNumber = string.IsNullOrWhiteSpace(item.BatchNumber)
                                ? $"BAT-{DateTime.UtcNow:yyyyMMddHHmmssfff}"
                                : item.BatchNumber,
                            ProductID = item.ProductID,
                            SupplierID = validSupplierId, // ✅ Safe SupplierID
                            PlantID = po.PlantID,
                            BinID = targetBinId, // ✅ Safe BinID
                            ManufacturingDate = item.ManufacturingDate ?? DateTime.Today,
                            ExpiryDate = expDate,
                            CurrentQuantity = item.ReceivingQty,
                            QCStatus = "Pending",
                            ReceivedDate = model.ReceivedDate,
                            CreatedByUserID = userId,
                            CreatedAt = DateTime.UtcNow
                        };

                        batches.Add(batch);
                    }

                    _context.InventoryBatches.AddRange(batches);
                    await _context.SaveChangesAsync();

                    var transactions = batches.Select(batch => new InventoryTransaction
                    {
                        BatchID = batch.BatchID,
                        PlantID = batch.PlantID,
                        TransactionType = "GRN",
                        Quantity = batch.CurrentQuantity,
                        ReferenceType = "PO",
                        ReferenceID = po.POID,
                        Remarks = $"GRN for PO {po.POCode}",
                        CreatedByUserID = userId,
                        Timestamp = DateTime.UtcNow
                    }).ToList();

                    _context.InventoryTransactions.AddRange(transactions);

                    bool allReceived = po.PurchaseOrderItems.All(x => x.ReceivedQty >= x.OrderedQty);
                    po.Status = allReceived ? "Received" : "PartiallyReceived";
                    po.ModifiedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (DbUpdateException ex)
                {
                    await transaction.RollbackAsync();
                    // Asal database error nikal kar controller ko bhejein
                    throw new InvalidOperationException($"Database error: {ex.InnerException?.Message ?? ex.Message}");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}