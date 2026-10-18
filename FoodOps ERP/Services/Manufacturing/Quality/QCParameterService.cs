using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Manufacturing.Quality.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.Quality;

public class QCParameterService : IQCParameterService
{
    private readonly ApplicationDbContext _db;

    public QCParameterService(ApplicationDbContext db) => _db = db;

    public async Task<List<QCParameterViewModel>> GetAllAsync(bool? activeOnly = null)
    {
        var query = _db.QCParameters.AsNoTracking().AsQueryable();

        if (activeOnly == true)
            query = query.Where(p => p.IsActive);

        return await query
            .OrderBy(p => p.ParameterCode)
            .Select(p => new QCParameterViewModel
            {
                ParameterID = p.ParameterID,
                ParameterCode = p.ParameterCode,
                ParameterName = p.ParameterName,
                ParameterType = p.ParameterType,
                DefaultUOMID = p.DefaultUOMID,
                DefaultUOMName = _db.UnitsOfMeasure.Where(u => u.UOMID == p.DefaultUOMID).Select(u => u.UOMName).FirstOrDefault(),
                IsActive = p.IsActive
            })
            .ToListAsync();
    }

    public async Task<QCParameterViewModel?> GetByIdAsync(int id)
    {
        var p = await _db.QCParameters.AsNoTracking().FirstOrDefaultAsync(x => x.ParameterID == id);
        if (p == null) return null;

        return new QCParameterViewModel
        {
            ParameterID = p.ParameterID,
            ParameterCode = p.ParameterCode,
            ParameterName = p.ParameterName,
            ParameterType = p.ParameterType,
            DefaultUOMID = p.DefaultUOMID,
            IsActive = p.IsActive
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(QCParameterViewModel model, int userId)
    {
        if (await _db.QCParameters.AnyAsync(p => p.ParameterCode == model.ParameterCode))
            return (false, "Parameter Code already exists.", null);

        if (await _db.QCParameters.AnyAsync(p => p.ParameterName == model.ParameterName))
            return (false, "Parameter Name already exists.", null);

        var entity = new QCParameter
        {
            ParameterCode = model.ParameterCode,
            ParameterName = model.ParameterName,
            ParameterType = model.ParameterType,
            DefaultUOMID = model.DefaultUOMID,
            IsActive = model.IsActive,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.QCParameters.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Parameter created successfully.", entity.ParameterID);
    }

    public async Task<(bool Success, string Message)> UpdateAsync(QCParameterViewModel model, int userId)
    {
        var entity = await _db.QCParameters.FindAsync(model.ParameterID);
        if (entity == null) return (false, "Parameter not found.");

        if (await _db.QCParameters.AnyAsync(p => p.ParameterCode == model.ParameterCode && p.ParameterID != model.ParameterID))
            return (false, "Parameter Code already exists.");

        if (await _db.QCParameters.AnyAsync(p => p.ParameterName == model.ParameterName && p.ParameterID != model.ParameterID))
            return (false, "Parameter Name already exists.");

        entity.ParameterCode = model.ParameterCode;
        entity.ParameterName = model.ParameterName;
        entity.ParameterType = model.ParameterType;
        entity.DefaultUOMID = model.DefaultUOMID;
        entity.IsActive = model.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Parameter updated successfully.");
    }

    public async Task<(bool Success, string Message)> ToggleActiveAsync(int id, int userId)
    {
        var entity = await _db.QCParameters.FindAsync(id);
        if (entity == null) return (false, "Parameter not found.");

        entity.IsActive = !entity.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, entity.IsActive ? "Parameter activated." : "Parameter deactivated.");
    }
}