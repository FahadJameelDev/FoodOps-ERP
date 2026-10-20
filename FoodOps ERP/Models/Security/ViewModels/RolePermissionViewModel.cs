namespace FoodOpsERP.Models.Security.ViewModels;

public class RolePermissionViewModel
{
    public int RoleID { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public List<PermissionCheckItem> Permissions { get; set; } = new();
}

public class PermissionCheckItem
{
    public int PermissionID { get; set; }
    public string PermissionCode { get; set; } = string.Empty;
    public string PermissionName { get; set; } = string.Empty;
    public string ModuleKey { get; set; } = string.Empty;
    public bool IsGranted { get; set; }
}