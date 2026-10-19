using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Manufacturing.Costing.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.Costing;

public class CostSetupService : ICostSetupService
{
    private readonly ApplicationDbContext _db;

    public CostSetupService(ApplicationDbContext db) => _db = db;

    // --- Components ---
    public async Task<List<CostComponentViewModel>> GetAllComponentsAsync(bool? activeOnly = null)
    {
        var query = _db.CostComponents.AsNoTracking().AsQueryable();
        if (activeOnly == true) query = query.Where(c => c.IsActive);

        return await query.OrderBy(c => c.ComponentCode)
            .Select(c => new CostComponentViewModel
            {
                ComponentID = c.ComponentID,
                ComponentCode = c.ComponentCode,
                ComponentName = c.ComponentName,
                ComponentType = c.ComponentType,
                AllocationBasis = c.AllocationBasis,
                IsActive = c.IsActive
            }).ToListAsync();
    }

    public async Task<CostComponentViewModel?> GetComponentByIdAsync(int id)
    {
        var c = await _db.CostComponents.AsNoTracking().FirstOrDefaultAsync(x => x.ComponentID == id);
        if (c == null) return null;

        return new CostComponentViewModel
        {
            ComponentID = c.ComponentID,
            ComponentCode = c.ComponentCode,
            ComponentName = c.ComponentName,
            ComponentType = c.ComponentType,
            AllocationBasis = c.AllocationBasis,
            IsActive = c.IsActive
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateComponentAsync(CostComponentViewModel model, int userId)
    {
        if (await _db.CostComponents.AnyAsync(c => c.ComponentCode == model.ComponentCode))
            return (false, "Component Code already exists.", null);

        var entity = new CostComponent
        {
            ComponentCode = model.ComponentCode,
            ComponentName = model.ComponentName,
            ComponentType = model.ComponentType,
            AllocationBasis = model.AllocationBasis,
            IsActive = model.IsActive,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.CostComponents.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Cost Component created successfully.", entity.ComponentID);
    }

    public async Task<(bool Success, string Message)> UpdateComponentAsync(CostComponentViewModel model, int userId)
    {
        var entity = await _db.CostComponents.FindAsync(model.ComponentID);
        if (entity == null) return (false, "Component not found.");

        if (await _db.CostComponents.AnyAsync(c => c.ComponentCode == model.ComponentCode && c.ComponentID != model.ComponentID))
            return (false, "Component Code already exists.");

        entity.ComponentCode = model.ComponentCode;
        entity.ComponentName = model.ComponentName;
        entity.ComponentType = model.ComponentType;
        entity.AllocationBasis = model.AllocationBasis;
        entity.IsActive = model.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Cost Component updated successfully.");
    }

    public async Task<(bool Success, string Message)> ToggleComponentActiveAsync(int id, int userId)
    {
        var entity = await _db.CostComponents.FindAsync(id);
        if (entity == null) return (false, "Component not found.");

        entity.IsActive = !entity.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, entity.IsActive ? "Component activated." : "Component deactivated.");
    }

    // --- Standard Costs ---
    public async Task<List<StandardCostViewModel>> GetAllStandardCostsAsync()
    {
        return await _db.StandardItemCosts.AsNoTracking()
            .OrderByDescending(s => s.EffectiveFrom)
            .Select(s => new StandardCostViewModel
            {
                CostID = s.CostID,
                ProductID = s.ProductID,
                ProductName = _db.Products.Where(p => p.ProductID == s.ProductID).Select(p => p.ProductName).FirstOrDefault(),
                EffectiveFrom = s.EffectiveFrom,
                EffectiveTo = s.EffectiveTo,
                StandardMaterialCost = s.StandardMaterialCost,
                StandardLaborCost = s.StandardLaborCost,
                StandardOverheadCost = s.StandardOverheadCost,
                TotalStandardCost = s.TotalStandardCost,
                CostUOMID = s.CostUOMID,
                CostUOMName = _db.UnitsOfMeasure.Where(u => u.UOMID == s.CostUOMID).Select(u => u.UOMName).FirstOrDefault(),
                Status = s.Status,
                Notes = s.Notes,
                CreatedAt = s.CreatedAt
            }).ToListAsync();
    }

    public async Task<StandardCostViewModel?> GetStandardCostByIdAsync(int id)
    {
        var s = await _db.StandardItemCosts.AsNoTracking().FirstOrDefaultAsync(x => x.CostID == id);
        if (s == null) return null;

        return new StandardCostViewModel
        {
            CostID = s.CostID,
            ProductID = s.ProductID,
            EffectiveFrom = s.EffectiveFrom,
            EffectiveTo = s.EffectiveTo,
            StandardMaterialCost = s.StandardMaterialCost,
            StandardLaborCost = s.StandardLaborCost,
            StandardOverheadCost = s.StandardOverheadCost,
            TotalStandardCost = s.TotalStandardCost,
            CostUOMID = s.CostUOMID,
            Status = s.Status,
            Notes = s.Notes
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateStandardCostAsync(StandardCostViewModel model, int userId)
    {
        var entity = new StandardItemCost
        {
            ProductID = model.ProductID,
            EffectiveFrom = model.EffectiveFrom,
            EffectiveTo = model.EffectiveTo,
            StandardMaterialCost = model.StandardMaterialCost,
            StandardLaborCost = model.StandardLaborCost,
            StandardOverheadCost = model.StandardOverheadCost,
            CostUOMID = model.CostUOMID,
            Status = "Draft",
            Notes = model.Notes,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.StandardItemCosts.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Standard Cost created successfully.", entity.CostID);
    }

    public async Task<(bool Success, string Message)> ApproveStandardCostAsync(int id, int userId)
    {
        var entity = await _db.StandardItemCosts.FindAsync(id);
        if (entity == null) return (false, "Cost record not found.");
        if (entity.Status == "Approved") return (false, "Already approved.");

        entity.Status = "Approved";
        entity.ApprovedByUserID = userId;
        entity.ApprovedAt = DateTime.UtcNow;

        // Optional: Supersede previous approved cost for this product
        var previous = await _db.StandardItemCosts
            .Where(s => s.ProductID == entity.ProductID && s.Status == "Approved" && s.CostID != id)
            .ToListAsync();

        foreach (var p in previous)
        {
            p.Status = "Superseded";
            p.EffectiveTo = entity.EffectiveFrom.AddDays(-1);
        }

        await _db.SaveChangesAsync();
        return (true, "Standard Cost approved.");
    }
}