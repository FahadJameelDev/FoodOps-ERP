using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;

public class RecipeService : IRecipeService
{
    private readonly ApplicationDbContext _db;

    public RecipeService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<RecipeViewModel>> GetAllAsync(string? status = null)
    {
        var query = _db.Recipes
            .AsNoTracking()
            .Include(r => r.Items)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status == status);

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new RecipeViewModel
            {
                RecipeID = r.RecipeID,
                ProductID = r.ProductID,
                ProductName = _db.Products.Where(p => p.ProductID == r.ProductID).Select(p => p.ProductName).FirstOrDefault(),
                VersionNo = r.VersionNo,
                RecipeCode = r.RecipeCode,
                Status = r.Status,
                StandardBatchSize = r.StandardBatchSize,
                BatchUOMID = r.BatchUOMID,
                BatchUOMName = _db.UnitsOfMeasure.Where(u => u.UOMID == r.BatchUOMID).Select(u => u.UOMName).FirstOrDefault(),
                YieldPercentage = r.YieldPercentage,
                EffectiveFrom = r.EffectiveFrom,
                EffectiveTo = r.EffectiveTo,
                IsCurrent = r.IsCurrent,
                ChangeSummary = r.ChangeSummary,
                ApprovedAt = r.ApprovedAt,
                CreatedAt = r.CreatedAt,
                Items = r.Items.Select(i => new RecipeItemViewModel
                {
                    RecipeItemID = i.RecipeItemID,
                    LineNo = i.LineNo,
                    IngredientProductID = i.IngredientProductID,
                    IngredientName = _db.Products.Where(p => p.ProductID == i.IngredientProductID).Select(p => p.ProductName).FirstOrDefault(),
                    RequiredQty = i.RequiredQty,
                    UOMID = i.UOMID,
                    UOMName = _db.UnitsOfMeasure.Where(u => u.UOMID == i.UOMID).Select(u => u.UOMName).FirstOrDefault(),
                    ScrapPercent = i.ScrapPercent,
                    IsCritical = i.IsCritical,
                    Remarks = i.Remarks
                }).ToList()
            }).ToListAsync();
    }

    public async Task<RecipeViewModel?> GetByIdAsync(int id)
    {
        var r = await _db.Recipes
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.RecipeID == id);

        if (r == null) return null;

        return new RecipeViewModel
        {
            RecipeID = r.RecipeID,
            ProductID = r.ProductID,
            ProductName = await _db.Products.Where(p => p.ProductID == r.ProductID).Select(p => p.ProductName).FirstOrDefaultAsync(),
            VersionNo = r.VersionNo,
            RecipeCode = r.RecipeCode,
            Status = r.Status,
            StandardBatchSize = r.StandardBatchSize,
            BatchUOMID = r.BatchUOMID,
            BatchUOMName = await _db.UnitsOfMeasure.Where(u => u.UOMID == r.BatchUOMID).Select(u => u.UOMName).FirstOrDefaultAsync(),
            YieldPercentage = r.YieldPercentage,
            EffectiveFrom = r.EffectiveFrom,
            EffectiveTo = r.EffectiveTo,
            IsCurrent = r.IsCurrent,
            ChangeSummary = r.ChangeSummary,
            ApprovedAt = r.ApprovedAt,
            CreatedAt = r.CreatedAt,
            Items = r.Items.OrderBy(i => i.LineNo).Select(i => new RecipeItemViewModel
            {
                RecipeItemID = i.RecipeItemID,
                RecipeID = i.RecipeID,
                LineNo = i.LineNo,
                IngredientProductID = i.IngredientProductID,
                IngredientName = _db.Products.Where(p => p.ProductID == i.IngredientProductID).Select(p => p.ProductName).FirstOrDefault(),
                RequiredQty = i.RequiredQty,
                UOMID = i.UOMID,
                UOMName = _db.UnitsOfMeasure.Where(u => u.UOMID == i.UOMID).Select(u => u.UOMName).FirstOrDefault(),
                ScrapPercent = i.ScrapPercent,
                IsCritical = i.IsCritical,
                Remarks = i.Remarks
            }).ToList()
        };
    }

    public async Task<List<RecipeViewModel>> GetPendingApprovalsAsync()
    {
        return await GetAllAsync(RecipeStatus.PendingApproval);
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(RecipeViewModel model, int userId)
    {
        if (await _db.Recipes.AnyAsync(r => r.RecipeCode == model.RecipeCode))
            return (false, "Recipe Code already exists.", null);

        var entity = new Recipe
        {
            ProductID = model.ProductID,
            VersionNo = model.VersionNo,
            RecipeCode = model.RecipeCode,
            Status = RecipeStatus.Draft,
            StandardBatchSize = model.StandardBatchSize,
            BatchUOMID = model.BatchUOMID,
            YieldPercentage = model.YieldPercentage,
            EffectiveFrom = model.EffectiveFrom,
            EffectiveTo = model.EffectiveTo,
            IsCurrent = false,
            ChangeSummary = model.ChangeSummary,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var item in model.Items ?? new List<RecipeItemViewModel>())
        {
            entity.Items.Add(new RecipeItem
            {
                LineNo = item.LineNo,
                IngredientProductID = item.IngredientProductID,
                RequiredQty = item.RequiredQty,
                UOMID = item.UOMID,
                ScrapPercent = item.ScrapPercent,
                IsCritical = item.IsCritical,
                Remarks = item.Remarks
            });
        }

        _db.Recipes.Add(entity);
        await _db.SaveChangesAsync();

        return (true, "Recipe created successfully.", entity.RecipeID);
    }

    public async Task<(bool Success, string Message)> UpdateAsync(RecipeViewModel model, int userId)
    {
        var entity = await _db.Recipes
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.RecipeID == model.RecipeID);

        if (entity == null)
            return (false, "Recipe not found.");

        if (entity.Status != RecipeStatus.Draft && entity.Status != RecipeStatus.Rejected)
            return (false, "Only Draft or Rejected recipes can be edited.");

        entity.StandardBatchSize = model.StandardBatchSize;
        entity.BatchUOMID = model.BatchUOMID;
        entity.YieldPercentage = model.YieldPercentage;
        entity.EffectiveFrom = model.EffectiveFrom;
        entity.EffectiveTo = model.EffectiveTo;
        entity.ChangeSummary = model.ChangeSummary;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        _db.RecipeItems.RemoveRange(entity.Items);
        entity.Items.Clear();

        foreach (var item in model.Items ?? new List<RecipeItemViewModel>())
        {
            entity.Items.Add(new RecipeItem
            {
                LineNo = item.LineNo,
                IngredientProductID = item.IngredientProductID,
                RequiredQty = item.RequiredQty,
                UOMID = item.UOMID,
                ScrapPercent = item.ScrapPercent,
                IsCritical = item.IsCritical,
                Remarks = item.Remarks
            });
        }

        await _db.SaveChangesAsync();
        return (true, "Recipe updated successfully.");
    }

    public async Task<(bool Success, string Message)> SubmitForApprovalAsync(int recipeId, int userId)
    {
        var entity = await _db.Recipes.FindAsync(recipeId);
        if (entity == null) return (false, "Recipe not found.");
        if (entity.Status != RecipeStatus.Draft && entity.Status != RecipeStatus.Rejected)
            return (false, "Only Draft/Rejected recipes can be submitted.");

        entity.Status = RecipeStatus.PendingApproval;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        _db.RecipeApprovals.Add(new RecipeApproval
        {
            RecipeID = recipeId,
            StepNo = 1,
            RequestedByUserID = userId,
            ApproverUserID = 0,
            Decision = ApprovalDecision.Pending,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return (true, "Submitted for approval.");
    }

    public async Task<(bool Success, string Message)> ApproveAsync(int recipeId, int approverId, string? comments)
    {
        var entity = await _db.Recipes.FindAsync(recipeId);
        if (entity == null) return (false, "Recipe not found.");
        if (entity.Status != RecipeStatus.PendingApproval)
            return (false, "Recipe is not pending approval.");
        if (entity.CreatedByUserID == approverId)
            return (false, "You cannot approve your own recipe.");

        entity.Status = RecipeStatus.Approved;
        entity.ApprovedByUserID = approverId;
        entity.ApprovedAt = DateTime.UtcNow;
        entity.ModifiedByUserID = approverId;
        entity.ModifiedAt = DateTime.UtcNow;

        var approval = await _db.RecipeApprovals
            .FirstOrDefaultAsync(a => a.RecipeID == recipeId && a.Decision == ApprovalDecision.Pending);

        if (approval != null)
        {
            approval.Decision = ApprovalDecision.Approved;
            approval.DecisionAt = DateTime.UtcNow;
            approval.ApproverUserID = approverId;
            approval.Comments = comments;
        }

        await _db.SaveChangesAsync();
        return (true, "Recipe approved successfully.");
    }

    public async Task<(bool Success, string Message)> RejectAsync(int recipeId, int approverId, string comments)
    {
        if (string.IsNullOrWhiteSpace(comments))
            return (false, "Comments are required for rejection.");

        var entity = await _db.Recipes.FindAsync(recipeId);
        if (entity == null) return (false, "Recipe not found.");
        if (entity.Status != RecipeStatus.PendingApproval)
            return (false, "Recipe is not pending approval.");

        entity.Status = RecipeStatus.Rejected;
        entity.ModifiedByUserID = approverId;
        entity.ModifiedAt = DateTime.UtcNow;

        var approval = await _db.RecipeApprovals
            .FirstOrDefaultAsync(a => a.RecipeID == recipeId && a.Decision == ApprovalDecision.Pending);

        if (approval != null)
        {
            approval.Decision = ApprovalDecision.Rejected;
            approval.DecisionAt = DateTime.UtcNow;
            approval.ApproverUserID = approverId;
            approval.Comments = comments;
        }

        await _db.SaveChangesAsync();
        return (true, "Recipe rejected.");
    }

    public async Task<(bool Success, string Message)> SetCurrentAsync(int recipeId, int userId)
    {
        var entity = await _db.Recipes.FindAsync(recipeId);
        if (entity == null) return (false, "Recipe not found.");
        if (entity.Status != RecipeStatus.Approved)
            return (false, "Only Approved recipes can be set as current.");

        var others = await _db.Recipes
            .Where(r => r.ProductID == entity.ProductID && r.IsCurrent)
            .ToListAsync();

        foreach (var o in others)
            o.IsCurrent = false;

        entity.IsCurrent = true;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Recipe set as current.");
    }
}