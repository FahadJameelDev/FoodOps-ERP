using FoodOpsERP.Models.Entities;

namespace FoodOpsERP.Models.Entities.Security;

public class RolePermission
{
    public int RolePermissionID { get; set; }
    public int RoleID { get; set; }
    public int PermissionID { get; set; }
    public DateTime GrantedAt { get; set; }
    public int GrantedBy { get; set; }

    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}