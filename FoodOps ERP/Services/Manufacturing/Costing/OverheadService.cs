using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Manufacturing.Costing.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.Costing;

public class OverheadService : IOverheadService
{
    private readonly ApplicationDbContext _db;

    public OverheadService(ApplicationDbContext db) => _db = db;

    public async Task<List<OverheadAllocationViewModel>> GetAllAsync()
    {
        return await _db.OverheadAllocations.AsNoTracking()
            .OrderByDescending(x => x.PeriodMonth)
            .Select(x => new OverheadAllocationViewModel
            {
                AllocationID = x.AllocationID,
                PlantID = x.PlantID,
                PlantName = _db.Plants.Where(p => p.PlantID == x.PlantID).Select(p => p.PlantName).FirstOrDefault(),
                ComponentID = x.ComponentID,
                ComponentName = _db.CostComponents.Where(c => c.ComponentID == x.ComponentID).Select(c => c.ComponentName).FirstOrDefault(),
                PeriodMonth = x.PeriodMonth,
                AllocatedAmount = x.AllocatedAmount,
                BasisQty = x.BasisQty,
                RatePerBasis = x.RatePerBasis,
                Status = x.Status
            }).ToListAsync();
    }

    public async Task<OverheadAllocationViewModel?> GetByIdAsync(int id)
    {
        var x = await _db.OverheadAllocations.AsNoTracking().FirstOrDefaultAsync(a => a.AllocationID == id);
        if (x == null) return null;

        return new OverheadAllocationViewModel
        {
            AllocationID = x.AllocationID,
            PlantID = x.PlantID,
            ComponentID = x.ComponentID,
            PeriodMonth = x.PeriodMonth,
            AllocatedAmount = x.AllocatedAmount,
            BasisQty = x.BasisQty,
            RatePerBasis = x.RatePerBasis,
            Status = x.Status
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(OverheadAllocationViewModel model, int userId)
    {
        if (await _db.OverheadAllocations.AnyAsync(a => a.PlantID == model.PlantID && a.ComponentID == model.ComponentID && a.PeriodMonth == model.PeriodMonth))
            return (false, "An allocation for this Plant, Component, and Month already exists.", null);

        var entity = new OverheadAllocation
        {
            PlantID = model.PlantID,
            ComponentID = model.ComponentID,
            PeriodMonth = new DateTime(model.PeriodMonth.Year, model.PeriodMonth.Month, 1),
            AllocatedAmount = model.AllocatedAmount,
            BasisQty = model.BasisQty,
            Status = "Draft",
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.OverheadAllocations.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Overhead Allocation created successfully.", entity.AllocationID);
    }

    public async Task<(bool Success, string Message)> PostAsync(int id, int userId)
    {
        var entity = await _db.OverheadAllocations.FindAsync(id);
        if (entity == null) return (false, "Allocation not found.");
        if (entity.Status == "Posted") return (false, "Already posted.");

        entity.Status = "Posted";
        entity.PostedByUserID = userId;
        entity.PostedAt = DateTime.UtcNow;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Overhead Allocation posted successfully.");
    }
}