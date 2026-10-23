using FoodOpsERP.Services.Inventory;
using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.ViewModels.Inventory;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Inventory
{
    public class IncomingQCService : IIncomingQCService
    {
        private readonly ApplicationDbContext _context;

        public IncomingQCService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LotViewModel>> GetPendingQCAsync()
        {
            return await _context.InventoryBatches
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.Supplier)
                .Where(x => x.QCStatus == "Pending")
                .OrderBy(x => x.ReceivedDate)
                .Select(x => new LotViewModel
                {
                    BatchID = x.BatchID,
                    BatchNumber = x.BatchNumber,
                    ProductName = x.Product.ProductName,
                    SupplierName = x.Supplier != null ? x.Supplier.SupplierName : null,
                    CurrentQuantity = x.CurrentQuantity,
                    ExpiryDate = x.ExpiryDate,
                    QCStatus = x.QCStatus
                }).ToListAsync();
        }

        public async Task<bool> PassQCAsync(long batchId, int inspectorUserId, string? remarks = null)
        {
            // ✅ Explicit check: Batch zaroor exist karna chahiye
            var batch = await _context.InventoryBatches.FindAsync(batchId);
            if (batch == null) throw new InvalidOperationException($"Batch ID {batchId} not found in database.");

            if (batch.QCStatus != "Pending" && batch.QCStatus != "Quarantine")
                throw new InvalidOperationException($"Cannot pass QC for batch in '{batch.QCStatus}' status.");

            // ✅ Explicit check: User zaroor exist karna chahiye, warna default 1 assign ho
            var validUser = await _context.Users.FindAsync(inspectorUserId);
            int finalInspectorId = (validUser != null) ? inspectorUserId : (await _context.Users.Select(u => u.UserID).FirstOrDefaultAsync());
            if (finalInspectorId == 0) throw new InvalidOperationException("No valid User found in Users table to assign as Inspector.");

            batch.QCStatus = "Passed";
            batch.ModifiedAt = DateTime.UtcNow;

            var inspection = new QCInspection
            {
                BatchID = batch.BatchID, // Ensure correct long ID
                InspectorUserID = finalInspectorId,
                InspectionType = "Incoming",
                OverallResult = "Pass",
                Remarks = remarks,
                InspectionDate = DateTime.UtcNow
            };

            _context.QCInspections.Add(inspection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> FailQCAsync(long batchId, int inspectorUserId, string remarks)
        {
            var batch = await _context.InventoryBatches.FindAsync(batchId);
            if (batch == null) throw new InvalidOperationException($"Batch ID {batchId} not found in database.");

            if (batch.QCStatus != "Pending" && batch.QCStatus != "Quarantine")
                throw new InvalidOperationException($"Cannot fail QC for batch in '{batch.QCStatus}' status.");

            var validUser = await _context.Users.FindAsync(inspectorUserId);
            int finalInspectorId = (validUser != null) ? inspectorUserId : (await _context.Users.Select(u => u.UserID).FirstOrDefaultAsync());
            if (finalInspectorId == 0) throw new InvalidOperationException("No valid User found in Users table to assign as Inspector.");

            batch.QCStatus = "Failed";
            batch.ModifiedAt = DateTime.UtcNow;

            var inspection = new QCInspection
            {
                BatchID = batch.BatchID,
                InspectorUserID = finalInspectorId,
                InspectionType = "Incoming",
                OverallResult = "Fail",
                Remarks = remarks,
                InspectionDate = DateTime.UtcNow
            };

            _context.QCInspections.Add(inspection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HoldBatchAsync(long batchId, int userId, string reason)
        {
            var batch = await _context.InventoryBatches.FindAsync(batchId);
            if (batch == null) return false;

            if (batch.QCStatus != "Pending" && batch.QCStatus != "Passed")
                throw new InvalidOperationException($"Cannot hold batch in '{batch.QCStatus}' status.");

            var validUser = await _context.Users.FindAsync(userId);
            int finalUserId = (validUser != null) ? userId : (await _context.Users.Select(u => u.UserID).FirstOrDefaultAsync());

            batch.QCStatus = "Quarantine";
            batch.ModifiedAt = DateTime.UtcNow;

            var hold = new HoldQuarantineLog
            {
                BatchID = batchId,
                HoldReason = reason,
                HeldByUserID = finalUserId > 0 ? finalUserId : 1,
                HeldAt = DateTime.UtcNow,
                Status = "OnHold"
            };

            _context.HoldQuarantineLogs.Add(hold);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReleaseBatchAsync(long batchId, int userId, string? remarks = null)
        {
            var batch = await _context.InventoryBatches.FindAsync(batchId);
            if (batch == null) return false;

            if (batch.QCStatus != "Quarantine")
                throw new InvalidOperationException($"Cannot release batch in '{batch.QCStatus}' status.");

            batch.QCStatus = "Released";
            batch.ModifiedAt = DateTime.UtcNow;

            var hold = await _context.HoldQuarantineLogs
                .Where(x => x.BatchID == batchId && x.Status == "OnHold")
                .OrderByDescending(x => x.HeldAt)
                .FirstOrDefaultAsync();

            if (hold == null)
                throw new InvalidOperationException($"No active hold found for batch {batchId}.");

            var validUser = await _context.Users.FindAsync(userId);
            int finalUserId = (validUser != null) ? userId : (await _context.Users.Select(u => u.UserID).FirstOrDefaultAsync());

            hold.Status = "Released";
            hold.ReleasedByUserID = finalUserId > 0 ? finalUserId : 1;
            hold.ReleasedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}