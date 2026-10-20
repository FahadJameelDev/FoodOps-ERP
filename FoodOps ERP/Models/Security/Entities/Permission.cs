using FoodOpsERP.Models.Entities.Manufacturing; // <-- YEH LINE ZAROORI HAI

namespace FoodOpsERP.Models.Entities.Security;

public class Permission : AuditedEntity
{
    public int PermissionID { get; set; }
    public string PermissionCode { get; set; } = string.Empty;
    public string PermissionName { get; set; } = string.Empty;
    public string ModuleKey { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}