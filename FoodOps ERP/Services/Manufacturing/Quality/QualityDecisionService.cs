using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Manufacturing.Quality.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.Quality;

public class QualityDecisionService : IQualityDecisionService
{
    private readonly ApplicationDbContext _db;

    public QualityDecisionService(ApplicationDbContext db) => _db = db;

    public async Task<List<QualityDecisionViewModel>> GetAllAsync(long? batchId = null)
    {
        var query = _db.QualityDecisions.AsNoTracking().AsQueryable();

        if (batchId.HasValue)
            query = query.Where(d => d.BatchID == batchId.Value);

        return await query
            .OrderByDescending(d => d.DecidedAt)
            .Select(d => new QualityDecisionViewModel
            {
                DecisionID = d.DecisionID,
                BatchID = d.BatchID,
                BatchNumber = _db.InventoryBatches.Where(b => b.BatchID == d.BatchID).Select(b => b.BatchNumber).FirstOrDefault(),
                InspectionID = d.InspectionID,
                NCRID = d.NCRID,
                Decision = d.Decision,
                Reason = d.Reason,
                DecidedByUserID = d.DecidedByUserID,
                DecidedByName = _db.Users.Where(u => u.UserID == d.DecidedByUserID).Select(u => u.Username).FirstOrDefault(),
                DecidedAt = d.DecidedAt,
                ApprovedByUserID = d.ApprovedByUserID,
                ApprovedByName = _db.Users.Where(u => u.UserID == d.ApprovedByUserID).Select(u => u.Username).FirstOrDefault(),
                ApprovedAt = d.ApprovedAt
            })
            .ToListAsync();
    }

    public async Task<QualityDecisionViewModel?> GetByIdAsync(int id)
    {
        var d = await _db.QualityDecisions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.DecisionID == id);

        if (d == null) return null;

        return new QualityDecisionViewModel
        {
            DecisionID = d.DecisionID,
            BatchID = d.BatchID,
            InspectionID = d.InspectionID,
            NCRID = d.NCRID,
            Decision = d.Decision,
            Reason = d.Reason,
            DecidedByUserID = d.DecidedByUserID,
            DecidedAt = d.DecidedAt,
            ApprovedByUserID = d.ApprovedByUserID,
            ApprovedAt = d.ApprovedAt
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(QualityDecisionViewModel model, int userId)
    {
        if (model.Decision != "Approve" && string.IsNullOrWhiteSpace(model.Reason))
            return (false, "Reason is required when Decision is not Approve.", null);

        var batch = await _db.InventoryBatches.FindAsync(model.BatchID);
        if (batch == null)
            return (false, "Batch not found.", null);

        var entity = new QualityDecision
        {
            BatchID = model.BatchID,
            InspectionID = model.InspectionID,
            NCRID = model.NCRID,
            Decision = model.Decision,
            Reason = model.Reason,
            DecidedByUserID = userId,
            DecidedAt = DateTime.UtcNow
        };

        _db.QualityDecisions.Add(entity);

        // Update Inventory Batch Status
        batch.QCStatus = model.Decision switch
        {
            "Approve" => "Approved",
            "Reject" => "Rejected",
            "Hold" => "Hold",
            "Rework" => "Rework",
            "ConditionalRelease" => "Conditional",
            _ => batch.QCStatus
        };

        await _db.SaveChangesAsync();
        return (true, "Quality decision recorded.", entity.DecisionID);
    }
}