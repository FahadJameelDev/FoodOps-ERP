using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.Manufacturing.ProductsAndRecipes.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Manufacturing.ProductsAndRecipes;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;
    public ProductService(ApplicationDbContext db) => _db = db;

    public async Task<List<ProductViewModel>> GetAllAsync()
    {
        return await _db.Products.AsNoTracking()
            .OrderBy(x => x.ProductName)
            .Select(x => new ProductViewModel
            {
                ProductID = x.ProductID,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                CategoryID = x.CategoryID,
                CategoryName = _db.Categories.Where(c => c.CategoryID == x.CategoryID).Select(c => c.CategoryName).FirstOrDefault(),
                BaseUOMID = x.BaseUOMID,
                UOMName = _db.UnitsOfMeasure.Where(u => u.UOMID == x.BaseUOMID).Select(u => u.UOMName).FirstOrDefault(),
                ProductType = x.ProductType ?? "Finished",
                IsActive = x.IsActive
            }).ToListAsync();
    }

    public async Task<ProductViewModel?> GetByIdAsync(int id)
    {
        var x = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductID == id);
        if (x == null) return null;

        return new ProductViewModel
        {
            ProductID = x.ProductID,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            CategoryID = x.CategoryID,
            BaseUOMID = x.BaseUOMID,
            ProductType = x.ProductType ?? "Finished",
            IsActive = x.IsActive
        };
    }

    public async Task<(bool Success, string Message)> CreateAsync(ProductViewModel model)
    {
        if (await _db.Products.AnyAsync(x => x.ProductCode == model.ProductCode))
            return (false, "Product code already exists.");

        var entity = new Product
        {
            ProductCode = model.ProductCode,
            ProductName = model.ProductName,
            CategoryID = model.CategoryID,
            BaseUOMID = model.BaseUOMID,
            ProductType = model.ProductType,
            IsActive = model.IsActive
        };
        _db.Products.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Product created successfully.");
    }

    public async Task<(bool Success, string Message)> UpdateAsync(ProductViewModel model)
    {
        var entity = await _db.Products.FindAsync(model.ProductID);
        if (entity == null) return (false, "Product not found.");

        entity.ProductName = model.ProductName;
        entity.CategoryID = model.CategoryID;
        entity.BaseUOMID = model.BaseUOMID;
        entity.ProductType = model.ProductType;
        entity.IsActive = model.IsActive;

        await _db.SaveChangesAsync();
        return (true, "Product updated successfully.");
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var entity = await _db.Products.FindAsync(id);
        if (entity == null) return (false, "Product not found.");

        // Note: Yahan foreign key check lagana chahiye taake agar product kisi recipe/PO mein use ho raha ho toh delete na ho
        _db.Products.Remove(entity);
        await _db.SaveChangesAsync();
        return (true, "Product deleted successfully.");
    }
}