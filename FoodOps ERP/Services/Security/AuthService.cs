using FoodOpsERP.Data;
using FoodOpsERP.Models.Security.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace FoodOpsERP.Services.Security;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;

    public AuthService(ApplicationDbContext db) => _db = db;

    public async Task<(bool Success, string Message, int? UserId)> ValidateLoginAsync(LoginViewModel model)
    {
        var user = await _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == model.Username);

        if (user == null)
            return (false, "Invalid username or password.", null);

        var hash = HashPassword(model.Password);
        if (user.PasswordHash != hash && user.PasswordHash != model.Password)
            return (false, "Invalid username or password.", null);

        return (true, "Login successful.", user.UserID);
    }

    public async Task<List<string>> GetUserRolesAsync(int userId)
    {
        return await _db.UserRoles.AsNoTracking()
            .Where(ur => ur.UserID == userId)
            .Join(_db.Roles.Where(r => r.IsActive),
                  ur => ur.RoleID, r => r.RoleID, (ur, r) => r.RoleCode)
            .ToListAsync();
    }

    public async Task<List<string>> GetUserPermissionsAsync(int userId)
    {
        var roleIds = await _db.UserRoles.AsNoTracking()
            .Where(ur => ur.UserID == userId)
            .Select(ur => ur.RoleID).ToListAsync();

        return await _db.RolePermissions.AsNoTracking()
            .Where(rp => roleIds.Contains(rp.RoleID))
            .Join(_db.Permissions.Where(p => p.IsActive),
                  rp => rp.PermissionID, p => p.PermissionID, (rp, p) => p.PermissionCode)
            .Distinct().ToListAsync();
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}