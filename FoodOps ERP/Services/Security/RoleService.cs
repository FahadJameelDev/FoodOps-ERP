using FoodOpsERP.Data;
using FoodOpsERP.Models.Entities.Security;
using FoodOpsERP.Models.Security.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FoodOpsERP.Services.Security;

public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _db;

    public RoleService(ApplicationDbContext db) => _db = db;

    public async Task<List<RoleViewModel>> GetAllRolesAsync()
    {
        return await _db.Roles.AsNoTracking()
            .OrderBy(r => r.RoleCode)
            .Select(r => new RoleViewModel
            {
                RoleID = r.RoleID,
                RoleCode = r.RoleCode,
                RoleName = r.RoleName,
                Description = r.Description,
                IsSystem = r.IsSystem,
                IsActive = r.IsActive
            }).ToListAsync();
    }

    public async Task<RoleViewModel?> GetRoleByIdAsync(int id)
    {
        var r = await _db.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.RoleID == id);
        if (r == null) return null;

        return new RoleViewModel
        {
            RoleID = r.RoleID,
            RoleCode = r.RoleCode,
            RoleName = r.RoleName,
            Description = r.Description,
            IsSystem = r.IsSystem,
            IsActive = r.IsActive
        };
    }

    public async Task<(bool Success, string Message, int? Id)> CreateRoleAsync(RoleViewModel model, int userId)
    {
        if (await _db.Roles.AnyAsync(r => r.RoleCode == model.RoleCode))
            return (false, "Role Code already exists.", null);

        var entity = new Role
        {
            RoleCode = model.RoleCode,
            RoleName = model.RoleName,
            Description = model.Description,
            IsSystem = false,
            IsActive = model.IsActive,
            CreatedByUserID = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Roles.Add(entity);
        await _db.SaveChangesAsync();
        return (true, "Role created.", entity.RoleID);
    }

    public async Task<(bool Success, string Message)> UpdateRoleAsync(RoleViewModel model, int userId)
    {
        var entity = await _db.Roles.FindAsync(model.RoleID);
        if (entity == null) return (false, "Role not found.");
        if (entity.IsSystem) return (false, "System roles cannot be edited.");

        if (await _db.Roles.AnyAsync(r => r.RoleCode == model.RoleCode && r.RoleID != model.RoleID))
            return (false, "Role Code already exists.");

        entity.RoleCode = model.RoleCode;
        entity.RoleName = model.RoleName;
        entity.Description = model.Description;
        entity.IsActive = model.IsActive;
        entity.ModifiedByUserID = userId;
        entity.ModifiedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return (true, "Role updated.");
    }

    public async Task<RolePermissionViewModel?> GetRolePermissionsAsync(int roleId)
    {
        var role = await _db.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleID == roleId);
        if (role == null) return null;

        var grantedIds = await _db.RolePermissions
            .AsNoTracking().Where(rp => rp.RoleID == roleId)
            .Select(rp => rp.PermissionID).ToListAsync();

        var permissions = await _db.Permissions.AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.ModuleKey).ThenBy(p => p.PermissionCode)
            .Select(p => new PermissionCheckItem
            {
                PermissionID = p.PermissionID,
                PermissionCode = p.PermissionCode,
                PermissionName = p.PermissionName,
                ModuleKey = p.ModuleKey,
                IsGranted = grantedIds.Contains(p.PermissionID)
            }).ToListAsync();

        return new RolePermissionViewModel
        {
            RoleID = role.RoleID,
            RoleName = role.RoleName,
            Permissions = permissions
        };
    }

    public async Task<(bool Success, string Message)> SaveRolePermissionsAsync(int roleId, List<int> permissionIds, int userId)
    {
        var role = await _db.Roles.FindAsync(roleId);
        if (role == null) return (false, "Role not found.");

        var existing = await _db.RolePermissions.Where(rp => rp.RoleID == roleId).ToListAsync();
        _db.RolePermissions.RemoveRange(existing);

        foreach (var pid in permissionIds.Distinct())
        {
            _db.RolePermissions.Add(new RolePermission
            {
                RoleID = roleId,
                PermissionID = pid,
                GrantedAt = DateTime.UtcNow,
                GrantedBy = userId
            });
        }

        await _db.SaveChangesAsync();
        return (true, "Permissions saved.");
    }
}