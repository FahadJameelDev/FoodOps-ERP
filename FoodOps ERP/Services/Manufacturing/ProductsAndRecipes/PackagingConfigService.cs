using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Manufacturing;
using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;

public class PackagingConfigService : IPackagingConfigService
{
    private readonly ApplicationDbContext _db;

    public PackagingConfigService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<PackagingConfigViewModel>> GetAllAsync(int? productId = null)
    {
        var query = _db.PackagingConfigs.AsNoTracking();

        if (productId.HasValue)
            query = query.Where(x => x.ProductID == productId.Value);

        return await query
            .OrderBy(x => x.ProductID)
            .Select(x => new PackagingConfigViewModel
            {
                PackagingConfigID = x.PackagingConfigID,
                ProductID = x.ProductID,
                ProductName = _db.Products.Where(p => p.ProductID == x.ProductID).Select(p => p.ProductName).FirstOrDefault(),
                PackageType = x.PackageType,
                PackageName = x.PackageName,
                UnitsPerPack = x.UnitsPerPack,
                NetWeight = x.NetWeight,
                GrossWeight = x.GrossWeight,
                WeightUOMID = x.WeightUOMID,
                WeightUOMName = _db.UnitsOfMeasure.Where(u => u.UOMID == x.WeightUOMID).Select(u => u.UOMName).FirstOrDefault(),
                Barcode = x.Barcode,
                IsDefault = x.IsDefault,
                IsActive = x.IsActive
            }).ToListAsync();
    }

    public async Task<PackagingConfigViewModel?> GetByIdAsync(int id)
    {
        var x = await _db.PackagingConfigs.AsNoTracking()
            .FirstOrDefaultAsync(p => p.PackagingConfigID == id);

        if (x == null) return null;

        return new PackagingConfigViewModel
        {
            PackagingConfigID = x.PackagingConfigID,
            ProductID = x.ProductID,
            ProductName = await _db.Products.Where(p => p.ProductID == x.ProductID).Select(p => p.ProductName).FirstOrDefaultAsync(),
            PackageType = x.PackageType,
            PackageName = x.PackageName,
            UnitsPerPack = x.UnitsPerPack,
            NetWeight = x.NetWeight,
            GrossWeight = x.GrossWeight,
            WeightUOMID = x.WeightUOMID,
            WeightUOMName = await _db.UnitsOfMeasure.Where(u => u.UOMID == x.WeightUOMID).Select(u => u.UOMName).FirstOrDefaultAsync(),
            Barcode = x.Barcode,
            IsDefault = x.IsDefault,
            IsActive = x.IsActive
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateAsync(PackagingConfigViewModel model, int userId)
    {
        if (model.GrossWeight < model.NetWeight)
            return (false, "Gross Weight cannot be less than Net Weight.", null);

        var entity = new PackagingConfig
        {
            ProductID = model.ProductID,
            PackageType = model.PackageType,
            PackageName = model.PackageName,
            UnitsPerPack = model.UnitsPerPack,
            NetWeight = model.NetWeight,
            GrossWeight = model.GrossWeight,
            WeightUOMID = model.WeightUOMID,
            Barcode = model.Barcode,
            IsDefault = model.IsDefault,
            IsActive = model.IsActive,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        if (model.IsDefault)
        {
            var others = await _db.PackagingConfigs
                .Where(p => p.ProductID == model.ProductID && p.IsDefault)
                .ToListAsync();
            foreach (var o in others) o.IsDefault = false;
        }

        _db.PackagingConfigs.Add(entity);
        await _db.SaveChangesAsync();

        return (true, "Packaging configuration created.", entity.PackagingConfigID);
    }

    public async Task<(bool Success, string Message)> UpdateAsync(PackagingConfigViewModel model, int userId)
    {
        var entity = await _db.PackagingConfigs.FindAsync(model.PackagingConfigID);
        if (entity == null) return (false, "Record not found.");

        if (model.GrossWeight < model.NetWeight)
            return (false, "Gross Weight cannot be less than Net Weight.");

        entity.PackageType = model.PackageType;
        entity.PackageName = model.PackageName;
        entity.UnitsPerPack = model.UnitsPerPack;
        entity.NetWeight = model.NetWeight;
        entity.GrossWeight = model.GrossWeight;
        entity.WeightUOMID = model.WeightUOMID;
        entity.Barcode = model.Barcode;
        entity.IsDefault = model.IsDefault;
        entity.IsActive = model.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        if (model.IsDefault)
        {
            var others = await _db.PackagingConfigs
                .Where(p => p.ProductID == entity.ProductID && p.PackagingConfigID != entity.PackagingConfigID && p.IsDefault)
                .ToListAsync();
            foreach (var o in others) o.IsDefault = false;
        }

        await _db.SaveChangesAsync();
        return (true, "Updated successfully.");
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var entity = await _db.PackagingConfigs.FindAsync(id);
        if (entity == null) return (false, "Record not found.");

        _db.PackagingConfigs.Remove(entity);
        await _db.SaveChangesAsync();
        return (true, "Deleted successfully.");
    }
}