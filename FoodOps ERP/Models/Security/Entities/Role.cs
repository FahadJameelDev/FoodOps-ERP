using FoodOpsERP.Models.Entities.Manufacturing; // <-- YEH LINE ZAROORI HAI

namespace FoodOpsERP.Models.Entities.Security;

public class Role : AuditedEntity
{
    public int RoleID { get; set; }
    public string RoleCode { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}