using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Manufacturing.Production.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.Production;

public class DemandService : IDemandService
{
    private readonly ApplicationDbContext _db;

    public DemandService(ApplicationDbContext db) => _db = db;

    public async Task<List<DemandViewModel>> GetAllAsync(string? status = null, int? plantId = null)
    {
        var query = _db.Demands.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(d => d.Status == status);

        if (plantId.HasValue)
            query = query.Where(d => d.PlantID == plantId.Value);

        return await query
            .OrderByDescending(d => d.RequiredDate)
            .Select(d => new DemandViewModel
            {
                DemandID = d.DemandID,
                DemandCode = d.DemandCode,
                PlantID = d.PlantID,
                PlantName = _db.Plants.Where(p => p.PlantID == d.PlantID).Select(p => p.PlantName).FirstOrDefault(),
                ProductID = d.ProductID,
                ProductName = _db.Products.Where(p => p.ProductID == d.ProductID).Select(p => p.ProductName).FirstOrDefault(),
                RequiredDate = d.RequiredDate,
                Quantity = d.Quantity,
                UOMID = d.UOMID,
                UOMName = _db.UnitsOfMeasure.Where(u => u.UOMID == d.UOMID).Select(u => u.UOMName).FirstOrDefault(),
                Source = d.Source,
                ReferenceNo = d.ReferenceNo,
                Priority = d.Priority,
                Status = d.Status,
                Notes = d.Notes,
                CreatedAt = d.CreatedAt
            }).ToListAsync();
    }

    public async Task<DemandViewModel?> GetByIdAsync(int id)
    {
        var d = await _db.Demands.AsNoTracking().FirstOrDefaultAsync(x => x.DemandID == id);
        if (d == null) return null;

        return new DemandViewModel
        {
            DemandID = d.DemandID,
            DemandCode = d.DemandCode,
            PlantID = d.PlantID,
            ProductID = d.ProductID,
            RequiredDate = d.RequiredDate,
            Quantity = d.Quantity,
            UOMID = d.UOMID,
            Source = d.Source,
            ReferenceNo = d.ReferenceNo,
            Priority = d.Priority,
            Status = d.Status,
            Notes = d.Notes,
            CreatedAt = d.CreatedAt
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(DemandViewModel model, int userId)
    {
        var lastCode = await _db.Demands.OrderByDescending(d => d.DemandID).Select(d => d.DemandCode).FirstOrDefaultAsync();
        var nextNumber = 1;
        if (!string.IsNullOrEmpty(lastCode) && lastCode.StartsWith("DEM-"))
        {
            if (int.TryParse(lastCode.Replace("DEM-", ""), out var n)) nextNumber = n + 1;
        }

        var entity = new Demand
        {
            DemandCode = $"DEM-{nextNumber:D5}",
            PlantID = model.PlantID,
            ProductID = model.ProductID,
            RequiredDate = model.RequiredDate,
            Quantity = model.Quantity,
            UOMID = model.UOMID,
            Source = model.Source,
            ReferenceNo = model.ReferenceNo,
            Priority = model.Priority,
            Status = "Open",
            Notes = model.Notes,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Demands.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Demand created successfully.", entity.DemandID);
    }

    public async Task<(bool Success, string Message)> UpdateAsync(DemandViewModel model, int userId)
    {
        var entity = await _db.Demands.FindAsync(model.DemandID);
        if (entity == null) return (false, "Demand not found.");
        if (entity.Status != "Open") return (false, "Only Open demands can be edited.");

        entity.PlantID = model.PlantID;
        entity.ProductID = model.ProductID;
        entity.RequiredDate = model.RequiredDate;
        entity.Quantity = model.Quantity;
        entity.UOMID = model.UOMID;
        entity.Source = model.Source;
        entity.ReferenceNo = model.ReferenceNo;
        entity.Priority = model.Priority;
        entity.Notes = model.Notes;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Demand updated successfully.");
    }

    public async Task<(bool Success, string Message)> CancelAsync(int id, int userId)
    {
        var entity = await _db.Demands.FindAsync(id);
        if (entity == null) return (false, "Demand not found.");
        if (entity.Status == "Fulfilled" || entity.Status == "Cancelled") return (false, "Cannot cancel this demand.");

        entity.Status = "Cancelled";
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Demand cancelled.");
    }
}