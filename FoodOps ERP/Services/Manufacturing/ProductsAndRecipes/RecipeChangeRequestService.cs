using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;

public class RecipeChangeRequestService : IRecipeChangeRequestService
{
    private readonly ApplicationDbContext _db;

    public RecipeChangeRequestService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<RecipeChangeRequestViewModel>> GetAllAsync(int? recipeId = null)
    {
        var query = _db.RecipeChangeRequests.AsNoTracking().AsQueryable();

        if (recipeId.HasValue)
            query = query.Where(x => x.RecipeID == recipeId.Value);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new RecipeChangeRequestViewModel
            {
                ChangeID = x.ChangeID,
                ChangeNo = x.ChangeNo,
                RecipeID = x.RecipeID,
                RecipeCode = _db.Recipes.Where(r => r.RecipeID == x.RecipeID).Select(r => r.RecipeCode).FirstOrDefault(),
                NewRecipeID = x.NewRecipeID,
                ChangeType = x.ChangeType,
                Reason = x.Reason,
                ImpactAssessment = x.ImpactAssessment,
                Status = x.Status,
                ApprovedAt = x.ApprovedAt,
                ImplementedAt = x.ImplementedAt,
                CreatedAt = x.CreatedAt
            }).ToListAsync();
    }

    public async Task<RecipeChangeRequestViewModel?> GetByIdAsync(int id)
    {
        var x = await _db.RecipeChangeRequests.AsNoTracking()
            .FirstOrDefaultAsync(r => r.ChangeID == id);

        if (x == null) return null;

        return new RecipeChangeRequestViewModel
        {
            ChangeID = x.ChangeID,
            ChangeNo = x.ChangeNo,
            RecipeID = x.RecipeID,
            RecipeCode = await _db.Recipes.Where(r => r.RecipeID == x.RecipeID).Select(r => r.RecipeCode).FirstOrDefaultAsync(),
            NewRecipeID = x.NewRecipeID,
            ChangeType = x.ChangeType,
            Reason = x.Reason,
            ImpactAssessment = x.ImpactAssessment,
            Status = x.Status,
            ApprovedAt = x.ApprovedAt,
            ImplementedAt = x.ImplementedAt,
            CreatedAt = x.CreatedAt
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(RecipeChangeRequestViewModel model, int userId)
    {
        // ChangeNo generate karna (e.g., CHG-0001)
        var count = await _db.RecipeChangeRequests.CountAsync() + 1;
        var changeNo = $"CHG-{count:D4}";

        var entity = new RecipeChangeRequest
        {
            ChangeNo = changeNo,
            RecipeID = model.RecipeID,
            ChangeType = model.ChangeType,
            Reason = model.Reason,
            ImpactAssessment = model.ImpactAssessment,
            Status = ChangeRequestStatus.Requested,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.RecipeChangeRequests.Add(entity);
        await _db.SaveChangesAsync();

        return (true, "Change Request submitted successfully.", entity.ChangeID);
    }

    public async Task<(bool Success, string Message)> ApproveAsync(int id, int approverId, string comments)
    {
        var entity = await _db.RecipeChangeRequests.FindAsync(id);
        if (entity == null) return (false, "Change Request not found.");
        if (entity.Status != ChangeRequestStatus.Requested && entity.Status != ChangeRequestStatus.UnderReview)
            return (false, "Only Requested or UnderReview changes can be approved.");

        entity.Status = ChangeRequestStatus.Approved;
        entity.ApprovedByUserID = approverId;
        entity.ApprovedAt = DateTime.UtcNow;
        entity.ModifiedByUserID = approverId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Change Request approved successfully.");
    }

    public async Task<(bool Success, string Message)> RejectAsync(int id, int approverId, string comments)
    {
        if (string.IsNullOrWhiteSpace(comments))
            return (false, "Comments are required for rejection.");

        var entity = await _db.RecipeChangeRequests.FindAsync(id);
        if (entity == null) return (false, "Change Request not found.");
        if (entity.Status != ChangeRequestStatus.Requested && entity.Status != ChangeRequestStatus.UnderReview)
            return (false, "Only Requested or UnderReview changes can be rejected.");

        entity.Status = ChangeRequestStatus.Rejected;
        // Yahan hum comments ko Reason ke sath append kar rahe hain taake history rahe
        entity.Reason += $" | Rejected: {comments}";
        entity.ModifiedByUserID = approverId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Change Request rejected.");
    }
}