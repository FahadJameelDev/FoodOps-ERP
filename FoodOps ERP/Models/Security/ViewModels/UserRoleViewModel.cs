namespace FoodOpsERP.Models.Security.ViewModels;

public class UserRoleViewModel
{
    public int UserID { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public List<RoleCheckItem> Roles { get; set; } = new();
}

public class RoleCheckItem
{
    public int RoleID { get; set; }
    public string RoleCode { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public bool IsAssigned { get; set; }
}