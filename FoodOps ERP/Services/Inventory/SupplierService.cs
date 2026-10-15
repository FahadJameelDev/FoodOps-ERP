using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities;
using FoodOpsERP.Models.ViewModels.Inventory;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Inventory
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _context;

        public SupplierService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupplierViewModel>> GetAllAsync()
        {
            return await _context.Suppliers
                .AsNoTracking()  // ✅ FIX #4
                .OrderBy(x => x.SupplierName)
                .Select(x => new SupplierViewModel
                {
                    SupplierID = x.SupplierID,
                    SupplierCode = x.SupplierCode,
                    SupplierName = x.SupplierName,
                    ContactPerson = x.ContactPerson,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = x.Address,
                    Rating = x.Rating,
                    IsActive = x.IsActive
                }).ToListAsync();
        }

        public async Task<SupplierViewModel?> GetByIdAsync(int id)
        {
            var entity = await _context.Suppliers
                .AsNoTracking()  // ✅ FIX #4
                .FirstOrDefaultAsync(x => x.SupplierID == id);

            if (entity == null) return null;

            return new SupplierViewModel
            {
                SupplierID = entity.SupplierID,
                SupplierCode = entity.SupplierCode,
                SupplierName = entity.SupplierName,
                ContactPerson = entity.ContactPerson,
                Email = entity.Email,
                Phone = entity.Phone,
                Address = entity.Address,
                Rating = entity.Rating,
                IsActive = entity.IsActive
            };
        }

        public async Task<bool> CreateAsync(SupplierViewModel model)
        {
            // ✅ FIX #2: Duplicate SupplierCode check
            var exists = await _context.Suppliers
                .AnyAsync(x => x.SupplierCode == model.SupplierCode);

            if (exists)
                throw new InvalidOperationException($"Supplier Code '{model.SupplierCode}' already exists.");

            var entity = new Supplier
            {
                SupplierCode = model.SupplierCode,
                SupplierName = model.SupplierName,
                ContactPerson = model.ContactPerson,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Rating = model.Rating,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Suppliers.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(SupplierViewModel model)
        {
            var entity = await _context.Suppliers.FindAsync(model.SupplierID);
            if (entity == null) return false;

            // ✅ FIX #5: Duplicate SupplierCode check (excluding self)
            var exists = await _context.Suppliers
                .AnyAsync(x => x.SupplierCode == model.SupplierCode
                            && x.SupplierID != model.SupplierID);

            if (exists)
                throw new InvalidOperationException($"Supplier Code '{model.SupplierCode}' is already used by another supplier.");

            entity.SupplierCode = model.SupplierCode;
            entity.SupplierName = model.SupplierName;
            entity.ContactPerson = model.ContactPerson;
            entity.Email = model.Email;
            entity.Phone = model.Phone;
            entity.Address = model.Address;
            entity.Rating = model.Rating;
            entity.IsActive = model.IsActive;
            entity.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Suppliers.FindAsync(id);
            if (entity == null) return false;

            // ✅ FIX #3: Check active transactions
            var hasActivePOs = await _context.PurchaseOrders
                .AnyAsync(x => x.SupplierID == id
                            && (x.Status == "Approved" || x.Status == "PartiallyReceived"));

            if (hasActivePOs)
                throw new InvalidOperationException(
                    "Cannot delete supplier with active purchase orders. Deactivate instead.");

            // ✅ FIX #3: Soft delete
            entity.IsActive = false;
            entity.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}