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
            var batch = await _context.InventoryBatches.FindAsync(batchId);
            if (batch == null) return false;

            // ✅ FIX: State machine — sirf Pending/Quarantine se Pass
            if (batch.QCStatus != "Pending" && batch.QCStatus != "Quarantine")
                throw new InvalidOperationException($"Cannot pass QC for batch in '{batch.QCStatus}' status. Only Pending or Quarantine batches can be passed.");

            batch.QCStatus = "Passed";
            batch.ModifiedAt = DateTime.UtcNow;

            var inspection = new QCInspection
            {
                BatchID = batchId,
                InspectorUserID = inspectorUserId,
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
            if (batch == null) return false;

            // ✅ FIX: State machine — sirf Pending/Quarantine se Fail
            if (batch.QCStatus != "Pending" && batch.QCStatus != "Quarantine")
                throw new InvalidOperationException($"Cannot fail QC for batch in '{batch.QCStatus}' status. Only Pending or Quarantine batches can be failed.");

            batch.QCStatus = "Failed";
            batch.ModifiedAt = DateTime.UtcNow;

            var inspection = new QCInspection
            {
                BatchID = batchId,
                InspectorUserID = inspectorUserId,
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

            // ✅ FIX: State machine — sirf Pending/Passed se Hold
            if (batch.QCStatus != "Pending" && batch.QCStatus != "Passed")
                throw new InvalidOperationException($"Cannot hold batch in '{batch.QCStatus}' status. Only Pending or Passed batches can be held.");

            batch.QCStatus = "Quarantine";
            batch.ModifiedAt = DateTime.UtcNow;

            var hold = new HoldQuarantineLog
            {
                BatchID = batchId,
                HoldReason = reason,
                HeldByUserID = userId,
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

            // ✅ FIX: State machine — sirf Quarantine se Release
            if (batch.QCStatus != "Quarantine")
                throw new InvalidOperationException($"Cannot release batch in '{batch.QCStatus}' status. Only Quarantine batches can be released.");

            batch.QCStatus = "Released";
            batch.ModifiedAt = DateTime.UtcNow;

            var hold = await _context.HoldQuarantineLogs
                .Where(x => x.BatchID == batchId && x.Status == "OnHold")
                .OrderByDescending(x => x.HeldAt)
                .FirstOrDefaultAsync();

            if (hold == null)
                throw new InvalidOperationException($"No active hold found for batch {batchId}.");

            hold.Status = "Released";
            hold.ReleasedByUserID = userId;
            hold.ReleasedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}