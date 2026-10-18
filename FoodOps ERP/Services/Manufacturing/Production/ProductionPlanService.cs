using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Manufacturing.Production.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.Production;

public class ProductionPlanService : IProductionPlanService
{
    private readonly ApplicationDbContext _db;

    public ProductionPlanService(ApplicationDbContext db) => _db = db;

    public async Task<List<ProductionPlanViewModel>> GetAllAsync(string? status = null, int? plantId = null)
    {
        var query = _db.ProductionPlans.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);

        if (plantId.HasValue)
            query = query.Where(p => p.PlantID == plantId.Value);

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProductionPlanViewModel
            {
                PlanID = p.PlanID,
                PlanCode = p.PlanCode,
                PlantID = p.PlantID,
                PlantName = _db.Plants.Where(x => x.PlantID == p.PlantID).Select(x => x.PlantName).FirstOrDefault(),
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                Notes = p.Notes,
                ApprovedAt = p.ApprovedAt,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ProductionPlanViewModel?> GetByIdAsync(int id)
    {
        var p = await _db.ProductionPlans
            .AsNoTracking()
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.PlanID == id);

        if (p == null) return null;

        return new ProductionPlanViewModel
        {
            PlanID = p.PlanID,
            PlanCode = p.PlanCode,
            PlantID = p.PlantID,
            PlantName = await _db.Plants.Where(x => x.PlantID == p.PlantID).Select(x => x.PlantName).FirstOrDefaultAsync(),
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            Status = p.Status,
            Notes = p.Notes,
            ApprovedAt = p.ApprovedAt,
            CreatedAt = p.CreatedAt,
            Lines = p.Lines.OrderBy(l => l.LineNo).Select(l => new ProductionPlanLineViewModel
            {
                PlanLineID = l.PlanLineID,
                PlanID = l.PlanID,
                LineNo = l.LineNo,
                ProductID = l.ProductID,
                ProductName = _db.Products.Where(x => x.ProductID == l.ProductID).Select(x => x.ProductName).FirstOrDefault(),
                RecipeID = l.RecipeID,
                RecipeCode = _db.Recipes.Where(x => x.RecipeID == l.RecipeID).Select(x => x.RecipeCode).FirstOrDefault(),
                PlannedQty = l.PlannedQty,
                UOMID = l.UOMID,
                UOMName = _db.UnitsOfMeasure.Where(x => x.UOMID == l.UOMID).Select(x => x.UOMName).FirstOrDefault(),
                PlannedDate = l.PlannedDate,
                DemandID = l.DemandID,
                Priority = l.Priority,
                Status = l.Status
            }).ToList()
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(ProductionPlanViewModel model, int userId)
    {
        if (model.EndDate < model.StartDate)
            return (false, "End Date cannot be before Start Date.", null);

        if (model.Lines == null || !model.Lines.Any())
            return (false, "At least one plan line is required.", null);

        var lastCode = await _db.ProductionPlans.OrderByDescending(p => p.PlanID).Select(p => p.PlanCode).FirstOrDefaultAsync();
        var nextNumber = 1;
        if (!string.IsNullOrEmpty(lastCode) && lastCode.StartsWith("PLN-"))
        {
            if (int.TryParse(lastCode.Replace("PLN-", ""), out var n)) nextNumber = n + 1;
        }

        var entity = new ProductionPlan
        {
            PlanCode = $"PLN-{nextNumber:D5}",
            PlantID = model.PlantID,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Status = "Draft",
            Notes = model.Notes,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        int lineNo = 1;
        foreach (var line in model.Lines)
        {
            entity.Lines.Add(new ProductionPlanLine
            {
                LineNo = lineNo++,
                ProductID = line.ProductID,
                RecipeID = line.RecipeID,
                PlannedQty = line.PlannedQty,
                UOMID = line.UOMID,
                PlannedDate = line.PlannedDate,
                DemandID = line.DemandID,
                Priority = line.Priority,
                Status = "Planned"
            });
        }

        _db.ProductionPlans.Add(entity);
        await _db.SaveChangesAsync();

        return (true, "Production Plan created successfully.", entity.PlanID);
    }

    public async Task<(bool Success, string Message)> UpdateAsync(ProductionPlanViewModel model, int userId)
    {
        var entity = await _db.ProductionPlans.Include(p => p.Lines).FirstOrDefaultAsync(p => p.PlanID == model.PlanID);
        if (entity == null) return (false, "Plan not found.");
        if (entity.Status != "Draft") return (false, "Only Draft plans can be edited.");

        entity.PlantID = model.PlantID;
        entity.StartDate = model.StartDate;
        entity.EndDate = model.EndDate;
        entity.Notes = model.Notes;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        _db.ProductionPlanLines.RemoveRange(entity.Lines);
        entity.Lines.Clear();

        int lineNo = 1;
        foreach (var line in model.Lines)
        {
            entity.Lines.Add(new ProductionPlanLine
            {
                LineNo = lineNo++,
                ProductID = line.ProductID,
                RecipeID = line.RecipeID,
                PlannedQty = line.PlannedQty,
                UOMID = line.UOMID,
                PlannedDate = line.PlannedDate,
                DemandID = line.DemandID,
                Priority = line.Priority,
                Status = "Planned"
            });
        }

        await _db.SaveChangesAsync();
        return (true, "Plan updated successfully.");
    }

    public async Task<(bool Success, string Message)> ApproveAsync(int planId, int userId)
    {
        var entity = await _db.ProductionPlans.FindAsync(planId);
        if (entity == null || entity.Status != "Draft") return (false, "Invalid plan or status.");

        entity.Status = "Approved";
        entity.ApprovedByUserID = userId;
        entity.ApprovedAt = DateTime.UtcNow;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Plan approved.");
    }

    public async Task<(bool Success, string Message)> ReleaseAsync(int planId, int userId)
    {
        var entity = await _db.ProductionPlans.FindAsync(planId);
        if (entity == null || entity.Status != "Approved") return (false, "Invalid plan or status.");

        entity.Status = "Released";
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Plan released.");
    }

    public async Task<(bool Success, string Message)> CloseAsync(int planId, int userId)
    {
        var entity = await _db.ProductionPlans.FindAsync(planId);
        if (entity == null || entity.Status != "Released") return (false, "Invalid plan or status.");

        entity.Status = "Closed";
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Plan closed.");
    }

    public async Task<(bool Success, string Message)> CancelAsync(int planId, int userId)
    {
        var entity = await _db.ProductionPlans.FindAsync(planId);
        if (entity == null || entity.Status == "Closed" || entity.Status == "Cancelled")
            return (false, "Cannot cancel this plan.");

        entity.Status = "Cancelled";
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Plan cancelled.");
    }
}