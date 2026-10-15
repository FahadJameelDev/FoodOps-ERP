using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.ViewModels.Inventory;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Inventory
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseOrderViewModel>> GetAllAsync()
        {
            return await _context.PurchaseOrders
                .AsNoTracking()
                .Include(x => x.Supplier)
                .OrderByDescending(x => x.OrderDate)
                .Select(x => new PurchaseOrderViewModel
                {
                    POID = x.POID,
                    POCode = x.POCode,
                    SupplierID = x.SupplierID,
                    SupplierName = x.Supplier.SupplierName,
                    PlantID = x.PlantID,
                    Status = x.Status,
                    OrderDate = x.OrderDate,
                    ExpectedDate = x.ExpectedDate,
                    TotalAmount = x.TotalAmount,
                    Remarks = x.Remarks
                }).ToListAsync();
        }

        public async Task<PurchaseOrderViewModel?> GetByIdAsync(int id)
        {
            var entity = await _context.PurchaseOrders
                .AsNoTracking()
                .Include(x => x.Supplier)
                .Include(x => x.PurchaseOrderItems).ThenInclude(i => i.Product)
                .Include(x => x.PurchaseOrderItems).ThenInclude(i => i.UOM)
                .FirstOrDefaultAsync(x => x.POID == id);

            if (entity == null) return null;

            return new PurchaseOrderViewModel
            {
                POID = entity.POID,
                POCode = entity.POCode,
                SupplierID = entity.SupplierID,
                SupplierName = entity.Supplier.SupplierName,
                PlantID = entity.PlantID,
                Status = entity.Status,
                OrderDate = entity.OrderDate,
                ExpectedDate = entity.ExpectedDate,
                TotalAmount = entity.TotalAmount,
                Remarks = entity.Remarks,
                Items = entity.PurchaseOrderItems.Select(i => new PurchaseOrderItemViewModel
                {
                    POItemID = i.POItemID,
                    ProductID = i.ProductID,
                    UOMID = i.UOMID,
                    OrderedQty = i.OrderedQty,
                    UnitCost = i.UnitCost,
                    ReceivedQty = i.ReceivedQty,
                    ProductName = i.Product.ProductName,
                    UOMCode = i.UOM.UOMCode
                }).ToList()
            };
        }

        public async Task<bool> CreateAsync(PurchaseOrderViewModel model)
        {
            var exists = await _context.PurchaseOrders
                .AnyAsync(x => x.POCode == model.POCode);
            if (exists)
                throw new InvalidOperationException($"PO Code '{model.POCode}' already exists.");

            var entity = new PurchaseOrder
            {
                POCode = model.POCode,
                SupplierID = model.SupplierID,
                PlantID = model.PlantID,
                Status = "Draft",
                OrderDate = model.OrderDate,
                ExpectedDate = model.ExpectedDate,
                TotalAmount = model.TotalAmount,
                Remarks = model.Remarks,
                CreatedAt = DateTime.UtcNow,
                PurchaseOrderItems = (model.Items ?? new()).Select(i => new PurchaseOrderItem
                {
                    ProductID = i.ProductID,
                    UOMID = i.UOMID,
                    OrderedQty = i.OrderedQty,
                    UnitCost = i.UnitCost,
                    ReceivedQty = 0,
                    CreatedAt = DateTime.UtcNow
                }).ToList()
            };

            _context.PurchaseOrders.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(PurchaseOrderViewModel model)
        {
            var entity = await _context.PurchaseOrders
                .Include(x => x.PurchaseOrderItems)
                .FirstOrDefaultAsync(x => x.POID == model.POID);

            if (entity == null) return false;

            if (entity.Status != "Draft" && entity.Status != "Submitted")
                throw new InvalidOperationException($"Cannot update PO in '{entity.Status}' status.");

            var exists = await _context.PurchaseOrders
                .AnyAsync(x => x.POCode == model.POCode && x.POID != model.POID);
            if (exists)
                throw new InvalidOperationException($"PO Code '{model.POCode}' already exists.");

            entity.POCode = model.POCode;
            entity.SupplierID = model.SupplierID;
            entity.PlantID = model.PlantID;
            entity.ExpectedDate = model.ExpectedDate;
            entity.TotalAmount = model.TotalAmount;
            entity.Remarks = model.Remarks;
            entity.ModifiedAt = DateTime.UtcNow;

            // Items replace karo
            _context.PurchaseOrderItems.RemoveRange(entity.PurchaseOrderItems);
            entity.PurchaseOrderItems = (model.Items ?? new()).Select(i => new PurchaseOrderItem
            {
                POID = entity.POID,
                ProductID = i.ProductID,
                UOMID = i.UOMID,
                OrderedQty = i.OrderedQty,
                UnitCost = i.UnitCost,
                ReceivedQty = i.ReceivedQty,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var entity = await _context.PurchaseOrders.FindAsync(id);
            if (entity == null) return false;

            if (entity.Status != "Draft" && entity.Status != "Submitted")
                throw new InvalidOperationException($"Cannot approve PO in '{entity.Status}' status.");

            var hasItems = await _context.PurchaseOrderItems.AnyAsync(x => x.POID == id);
            if (!hasItems)
                throw new InvalidOperationException("Cannot approve PO without items.");

            entity.Status = "Approved";
            entity.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.PurchaseOrders.FindAsync(id);
            if (entity == null) return false;

            if (entity.Status != "Draft")
                throw new InvalidOperationException($"Cannot delete PO in '{entity.Status}' status.");

            _context.PurchaseOrders.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}