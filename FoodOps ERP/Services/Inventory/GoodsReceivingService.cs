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
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var po = await _context.PurchaseOrders
                    .Include(x => x.PurchaseOrderItems)
                    .FirstOrDefaultAsync(x => x.POID == model.POID);

                if (po == null)
                    throw new InvalidOperationException($"PO ID {model.POID} not found.");

                // ✅ FIX: Sirf Approved/PartiallyReceived se GRN
                if (po.Status != "Approved" && po.Status != "PartiallyReceived")
                    throw new InvalidOperationException($"Cannot create GRN for PO in '{po.Status}' status.");

                // ✅ FIX: Kam se kam ek item receiving
                var receivingItems = model.Items.Where(x => x.ReceivingQty > 0).ToList();
                if (!receivingItems.Any())
                    throw new InvalidOperationException("At least one item must have receiving quantity > 0.");

                var batches = new List<InventoryBatch>();

                foreach (var item in receivingItems)
                {
                    var poItem = po.PurchaseOrderItems
                        .FirstOrDefault(x => x.POItemID == item.POItemID)
                        ?? throw new InvalidOperationException($"PO Item {item.POItemID} not found.");

                    // ✅ FIX: Over-receipt check
                    if (poItem.ReceivedQty + item.ReceivingQty > poItem.OrderedQty)
                        throw new InvalidOperationException(
                            $"Over-receipt for '{item.ProductName}'. Ordered: {poItem.OrderedQty}, Already received: {poItem.ReceivedQty}, Trying to receive: {item.ReceivingQty}");

                    // ✅ FIX: Expiry date validation
                    if (item.ExpiryDate.HasValue && item.ExpiryDate.Value.Date <= DateTime.Today)
                        throw new InvalidOperationException(
                            $"Expiry date for '{item.ProductName}' must be in the future.");

                    poItem.ReceivedQty += item.ReceivingQty;

                    var batch = new InventoryBatch
                    {
                        // ✅ FIX: Unique batch number with milliseconds
                        BatchNumber = string.IsNullOrWhiteSpace(item.BatchNumber)
                            ? $"BAT-{DateTime.UtcNow:yyyyMMddHHmmssfff}"
                            : item.BatchNumber,
                        ProductID = item.ProductID,
                        SupplierID = model.SupplierID,
                        PlantID = po.PlantID,
                        ManufacturingDate = item.ManufacturingDate,
                        ExpiryDate = item.ExpiryDate,
                        CurrentQuantity = item.ReceivingQty,
                        QCStatus = "Pending",
                        ReceivedDate = model.ReceivedDate,
                        CreatedByUserID = userId,
                        CreatedAt = DateTime.UtcNow
                    };

                    batches.Add(batch);
                }

                // ✅ FIX: Batch save — loop ke bahar, ek dafa
                _context.InventoryBatches.AddRange(batches);
                await _context.SaveChangesAsync();

                // ✅ FIX: Transactions ek saath
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

                // Update PO Status
                bool allReceived = po.PurchaseOrderItems.All(x => x.ReceivedQty >= x.OrderedQty);
                po.Status = allReceived ? "Received" : "PartiallyReceived";
                po.ModifiedAt = DateTime.UtcNow;

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